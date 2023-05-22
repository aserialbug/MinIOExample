using MinIOExample.Application.Models;

namespace MinIOExample.Application.Interfaces;

public interface IFileMetadataRepository : IRepository<FileMetadata, FileId>
{
    Task<FileMetadata[]> GetTemporaryObjects();
    Task<FileMetadata[]> GetByIds(IEnumerable<FileId> fileIds);
}