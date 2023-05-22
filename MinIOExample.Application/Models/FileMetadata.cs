using System;
using System.IO;

namespace MinIOExample.Application.Models;

public class FileMetadata : Entity<FileId>
{
    public string FileName { get; }
    public string FileExt { get; }
    public long FileSizeBytes { get; }
    public ContentType ContentType { get; }
    public DateTime UploadedAt { get; }
    public UserId UploadedBy { get; }
    public bool IsTemporary { get; private set; }

    public FileMetadata(FileId id, 
        string fileName, 
        string fileExt, 
        long fileSizeBytes, 
        ContentType contentType, 
        DateTime uploadedAt, 
        UserId uploadedBy, 
        bool isTemporary) : base(id)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentNullException(nameof(fileName));
        FileName = fileName;
        
        if (string.IsNullOrWhiteSpace(fileExt))
            throw new ArgumentNullException(nameof(fileExt));
        FileExt = fileExt;
        
        FileSizeBytes = fileSizeBytes;

        ContentType = contentType 
                      ?? throw new ArgumentNullException(nameof(contentType));
        
        UploadedAt = uploadedAt;
        UploadedBy = uploadedBy 
                     ?? throw new ArgumentNullException(nameof(uploadedBy));
        IsTemporary = isTemporary;
    }

    public void StorePermanently()
    {
        IsTemporary = false;
    }

    public static FileMetadata New(string fileName, long fileSize, ContentType contentType,
        UserId uploadedBy)
    {
        return new FileMetadata(FileId.New(),
            Path.GetFileNameWithoutExtension(fileName),
            Path.GetExtension(fileName),
            fileSize,
            contentType,
            DateTime.UtcNow,
            uploadedBy,
            true);
    }
}