using MinIOExample.Application.Exceptions;
using MinIOExample.Application.Interfaces;
using MinIOExample.Application.Models;
using MinIOExample.Infrastructure.Context;

namespace MinIOExample.Infrastructure.Repositories;

public class FileMetadataRepository : IFileMetadataRepository
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
        await _context.SaveChangesAsync(token);
    }

    public async Task RemoveAsync(FileMetadata metadata, CancellationToken token = default)
    {
        _context.Metadata.Remove(metadata);
        await _context.SaveChangesAsync(token);
    }
}