using MinIOExample.Application.Models;

namespace MinIOExample.Application.Exceptions;

public class StoredFileNotFound : Exception
{
    public FileId FileId { get; }
    
    public StoredFileNotFound(FileId fileId) : base($"File with Id={fileId} was not found")
    {
        FileId = fileId;
    }
}