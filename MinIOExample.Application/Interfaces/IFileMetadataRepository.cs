using MinIOExample.Application.Models;

namespace MinIOExample.Application.Interfaces;

public interface IFileMetadataRepository : IRepository<FileMetadata, FileId>
{
    Task<FileMetadata[]> GetTemporaryObjectsAsync(CancellationToken token = default);
    Task<FileMetadata[]> GetByIds(IEnumerable<FileId> fileIds, CancellationToken token = default);
}