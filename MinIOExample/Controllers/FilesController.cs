using Microsoft.AspNetCore.Mvc;
using MinIOExample.Application.Models;
using MinIOExample.Application.Models.DTO;
using MinIOExample.Application.Services;
using MinIOExample.Commands;
using MinIOExample.Extensions;
using MinIOExample.ViewModels;

namespace MinIOExample.Controllers;

/// <summary>
/// Контроллер для работы с файлами
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class FilesController : ControllerBase
{
    private readonly FileService _fileService;

    /// <summary>
    /// Конструктор контроллера
    /// </summary>
    /// <param name="fileService"></param>
    public FilesController(FileService fileService)
    {
        _fileService = fileService;
    }

    /// <summary>
    /// Загрузить файл
    /// </summary>
    /// <param name="fileId">Идентификатор файла</param>
    /// <param name="token">Токен отмены операции</param>
    /// <returns></returns>
    [HttpGet("{fileId}")]
    [ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<FileStreamResult> DownloadFileAsync(string fileId, CancellationToken token)
    {
        var id = FileId.Parse(fileId);
        var file = await _fileService.DownloadAsync(id, token);
        return new FileStreamResult(file.Content, file.ContentType.ToString())
        {
            FileDownloadName = file.FileName
        };
    }

    /// <summary>
    /// Получение сведений о файле
    /// </summary>
    /// <param name="fileId">Идентификатор файла</param>
    /// <param name="token">Токен отмены операции</param>
    /// <returns></returns>
    [HttpGet("{fileId}/metadata")]
    [ProducesResponseType(typeof(FileMetadataViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<FileMetadataViewModel> GetFileMetaAsync(string fileId, CancellationToken token)
    {
        var id = FileId.Parse(fileId);
        var metadata = await _fileService.GetMetadataAsync(id, token);
        return metadata.ToViewModel();
    }

    /// <summary>
    /// Поместить файл в хранилище
    /// </summary>
    /// <param name="formFile">Файл</param>
    /// <param name="token">Токен отмены операции</param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(FileMetadataViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<FileMetadataViewModel> UploadFileAsync(IFormFile formFile, CancellationToken token)
    {
        using var memory = new MemoryStream();
        await formFile.CopyToAsync(memory, token);
    
        var file = new FileDto
        {
            FileName = formFile.FileName,
            ContentType = new ContentType(formFile.ContentType),
            Content = memory
        };

        var metadata = await _fileService.UploadAsync(file, token);
        return metadata.ToViewModel();
    }

    /// <summary>
    /// Удалить файл из хранилища
    /// </summary>
    /// <param name="fileId">Идентификатор файла</param>
    /// <param name="token">Токен отмены операции</param>
    /// <returns></returns>
    [HttpDelete("{fileId}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task DeleteFileAsync(string fileId, CancellationToken token)
    {
        var id = FileId.Parse(fileId);
        await _fileService.DeleteAsync(id, token);
    }

    /// <summary>
    /// Помечает файлы для постоянного хранения
    /// </summary>
    /// <param name="command"></param>
    [HttpPost("Accept")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task AcceptToPermanentStorage(AcceptToPermanentStorageCommand command)
    {
        var fileIds = command.FileIds.Select(FileId.Parse);
        await _fileService.AcceptToPermanentStorage(fileIds);
    }
}