using MinIOExample.Application.Models;
using MinIOExample.ViewModels;

namespace MinIOExample.Extensions;

public static class FileMetadataExtensions
{
    public static FileMetadataViewModel ToViewModel(this FileMetadata metadata)
    {
        return new FileMetadataViewModel
        {
            Id = metadata.Id.ToString(),
            FileName = metadata.FileName,
            FileExt = metadata.FileExt,
            ContentType = metadata.ContentType.ToString(),
            FileSize = metadata.FileSizeBytes,
            UploadedAt = metadata.UploadedAt,
            UploadedBy = metadata.UploadedBy.ToString()
        };
    }
}