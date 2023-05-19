using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Minio;
using Minio.Exceptions;
using MinIOExample.Application.Exceptions;
using MinIOExample.Application.Interfaces;
using MinIOExample.Application.Models;
using MinIOExample.Infrastructure.Settings;

namespace MinIOExample.Infrastructure.Repositories;

public class FileContentRepository : IFileContentRepository
{
    private readonly MinioSettings _minioSettings;
    private readonly MinioClient _minioClient;
    private readonly SemaphoreSlim _semaphore = new (1, 1);

    public FileContentRepository(IOptions<MinioSettings> minioSettings)
    {
        _minioSettings = minioSettings.Value;
        _minioClient = new MinioClient()
            .WithEndpoint(_minioSettings.Endpoint)
            .WithCredentials(_minioSettings.AccessKey, _minioSettings.SecretKey)
            .WithSSL(false)
            .Build();
    }

    public Task<FileContent> this[FileId id] => GetById(id);

    private async Task<FileContent> GetById(FileId fileId, CancellationToken token = default)
    {
        MemoryStream fileContent = new MemoryStream();
        var args = new GetObjectArgs()
            .WithBucket(_minioSettings.Bucket)
            .WithObject(fileId.ToString())
            .WithCallbackStream(stream =>
            {
                stream.CopyTo(fileContent);
                fileContent.Seek(0, SeekOrigin.Begin);
                _semaphore.Release();
            });

        try
        {
            var stat = await _minioClient.GetObjectAsync(args, token);
            await _semaphore.WaitAsync(token);
        
            return new FileContent(fileId, fileContent, new ContentType(stat.ContentType));
        }
        catch (ObjectNotFoundException _)
        {
            throw new StoredFileNotFound(fileId);
        }
    }
    
    public async Task AddAsync(FileContent entity, CancellationToken token = default)
    {
        var args = new PutObjectArgs()
            .WithBucket(_minioSettings.Bucket)
            .WithObject(entity.Id.ToString())
            .WithContentType(entity.ContentType.ToString())
            .WithObjectSize(entity.Content.Length)
            .WithStreamData(entity.Content);

        await _minioClient.PutObjectAsync(args, token);
    }

    public Task RemoveAsync(FileContent entity, CancellationToken token = default)
    {
        return RemoveByIdAsync(entity.Id, token);
    }

    public async Task RemoveByIdAsync(FileId fileId, CancellationToken token = default)
    {
        var args = new RemoveObjectArgs()
            .WithBucket(_minioSettings.Bucket)
            .WithObject(fileId.ToString());

        // В текущей версии клиента MinIO есил объект не найден
        // в хранилище, то ошибки не возникает и метод завершается
        // успешно
        await _minioClient.RemoveObjectAsync(args, token);
    }
}