using Microsoft.Extensions.Options;
using MinIOExample.Application.Exceptions;
using MinIOExample.Application.Interfaces;
using MinIOExample.Application.Models;
using MinIOExample.Application.Models.DTO;
using MinIOExample.Application.Settings;
using MinIOExample.Infrastructure;

namespace MinIOExample.Application.Services;

public class FileService
{
   private readonly IFileContentRepository _contentRepository;
   private readonly IFileMetadataRepository _metadataRepository;
   private readonly UploadRestrictionsSettings _restrictionsSettings;

   public FileService(
      IFileContentRepository contentRepository, 
      IFileMetadataRepository metadataRepository,
      IOptions<UploadRestrictionsSettings> uploadRestrictionsSettings)
   {
      _contentRepository = contentRepository;
      _metadataRepository = metadataRepository;
      _restrictionsSettings = uploadRestrictionsSettings.Value;
   }

   public async Task<FileMetadata> GetMetadataAsync(FileId fileId, CancellationToken token = default)
   {
      return await _metadataRepository[fileId];
   }

   public async Task<FileDto> DownloadAsync(FileId fileId, CancellationToken token = default)
   {
      var contentTask = _contentRepository[fileId];
      var metadataTask = _metadataRepository[fileId];

      await Task.WhenAll(contentTask, metadataTask);

      var content = await contentTask;
      var metadata = await metadataTask;

      return new FileDto
      {
         Content = content.Content,
         FileName = metadata.FileName + metadata.FileExt,
         ContentType = metadata.ContentType
      };
   }

   public async Task<FileMetadata> UploadAsync(FileDto file, CancellationToken token = default)
   {
      var metadata = FileMetadata.New(file.FileName, file.Content.Length, file.ContentType, UserId.Admin);
      
      // Переместим курсор на начала файлового потока
      file.Content.Seek(0, SeekOrigin.Begin);
      
      var fileSizeMiB = metadata.FileSizeBytes * 1d / Constants.Mebibyte;
      if (_restrictionsSettings.MaxFileSizeMiB.HasValue && fileSizeMiB > _restrictionsSettings.MaxFileSizeMiB)
         throw new PolicyViolationException($"File size exceeds MaxFileSizeMb={_restrictionsSettings.MaxFileSizeMiB} threshold");
        
      var extension = Path.GetExtension(file.FileName);
      if (_restrictionsSettings.AllowedFileTypes != null && !_restrictionsSettings.AllowedFileTypes.Contains(extension))
         throw new PolicyViolationException($"File type {extension} is not in AllowedFileTypes");

      var content = new FileContent(metadata.Id, file.Content, file.ContentType);
      try
      {
         await Task.WhenAll(
            _metadataRepository.AddAsync(metadata, token),
            _contentRepository.AddAsync(content, token));
      }
      catch (Exception e)
      {
         try
         {
            // Пытаемся откатить создание объектов, но особо не паримся 
            // из-за результатов этого отката
            _ = _metadataRepository.RemoveAsync(metadata, token);
            _ = _contentRepository.RemoveAsync(content, token);
         }
         catch
         {
            // ignored
         }
         throw;
      }
      
      return metadata;
   }

   public async Task DeleteAsync(FileId fileId, CancellationToken token = default)
   {
      var metadata = await _metadataRepository[fileId];

      await Task.WhenAll(
         _contentRepository.RemoveByIdAsync(fileId, token),
         _metadataRepository.RemoveAsync(metadata, token));
   }

   public async Task AcceptToPermanentStorage(IEnumerable<FileId> fileIds)
   {
      foreach (var fileMetadata in await _metadataRepository.GetByIds(fileIds))
      {
         fileMetadata.StorePermanently();
      }
   }
}