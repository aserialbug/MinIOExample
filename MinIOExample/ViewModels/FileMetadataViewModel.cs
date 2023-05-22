using System;

namespace MinIOExample.ViewModels;

public record FileMetadataViewModel
{
    public string Id { get; init; }
    public string FileName { get; init; }
    public string FileExt { get; init; }
    public long FileSize { get; init; }
    public string ContentType { get; init; }
    public DateTime UploadedAt { get; init; }
    public string UploadedBy { get; init; }
    public bool IsTempFile { get; init; }
}