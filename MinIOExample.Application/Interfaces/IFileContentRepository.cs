using MinIOExample.Application.Models;

namespace MinIOExample.Application.Interfaces;

public interface IFileContentRepository : IRepository<FileContent, FileId>
{
    public Task RemoveByIdAsync(FileId fileId, CancellationToken token = default);
}