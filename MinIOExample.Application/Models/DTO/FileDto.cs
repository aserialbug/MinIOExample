using System.IO;

namespace MinIOExample.Application.Models.DTO;

public record FileDto
{
    public string FileName { get; init; }
    public ContentType ContentType { get; init; }
    public Stream Content { get; init; }
}