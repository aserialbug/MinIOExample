using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MinIOExample.Application.Models;

namespace MinIOExample.Infrastructure.Context.Configurations;

public class FileMetadataEntityTypeConfiguration : IEntityTypeConfiguration<FileMetadata>
{
    public void Configure(EntityTypeBuilder<FileMetadata> builder)
    {
        builder.ToTable("FileMetadata");
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id)
            .HasConversion(
                fileId => fileId.ToString(), 
                value => FileId.Parse(value));

        builder.Property(m => m.FileName);
        builder.Property(m => m.FileExt);
        builder.Property(m => m.FileSizeBytes);
        builder.Property(m => m.ContentType)
            .HasConversion(
                ct => ct.ToString(),
                value => new ContentType(value));

        builder.Property(m => m.UploadedAt);
        builder.Property(m => m.UploadedBy)
            .HasConversion(
                userId => userId.ToString(),
                value => new UserId(value));
    }
}