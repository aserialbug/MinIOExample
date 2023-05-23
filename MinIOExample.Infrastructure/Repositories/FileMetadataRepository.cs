using Microsoft.EntityFrameworkCore;
using MinIOExample.Application.Exceptions;
using MinIOExample.Application.Interfaces;
using MinIOExample.Application.Models;
using MinIOExample.Infrastructure.Context;

namespace MinIOExample.Infrastructure.Repositories;

public class FileMetadataRepository : IFileMetadataRepository, IDisposable
{
    private readonly ApplicationDbContext _context;

    public FileMetadataRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<FileMetadata> this[FileId id] => GetById(id);
    
    private async Task<FileMetadata> GetById(FileId fileId)
    {
        return await _context.Metadata
                   .FindAsync(fileId) 
               ?? throw new StoredFileNotFound(fileId);
    }

    public async Task AddAsync(FileMetadata metadata, CancellationToken token = default)
    {
        await _context.Metadata.AddAsync(metadata, token);
    }

    public Task RemoveAsync(FileMetadata metadata, CancellationToken token = default)
    {
        _context.Metadata.Remove(metadata);
        return Task.CompletedTask;
    }

    public async Task<FileMetadata[]> GetTemporaryObjects(CancellationToken token = default)
    {
        return await _context.Metadata
            .Where(m => m.IsTemporary)
            .ToArrayAsync(token);
    }

    public async Task<FileMetadata[]> GetByIds(IEnumerable<FileId> fileIds, CancellationToken token = default)
    {
        return await _context.Metadata
            .Where(m => fileIds.Contains(m.Id))
            .ToArrayAsync(token);
    }

    public void Dispose()
    {
        _context.SaveChanges();
        _context.Dispose();
    }
}