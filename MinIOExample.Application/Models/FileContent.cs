using System.IO;

namespace MinIOExample.Application.Models;

public class FileContent : Entity<FileId>
{
    public Stream Content { get; }
    public ContentType ContentType { get; }
    
    public FileContent(FileId id, Stream content, ContentType contentType) : base(id)
    {
        Content = content;
        ContentType = contentType;
    }
}