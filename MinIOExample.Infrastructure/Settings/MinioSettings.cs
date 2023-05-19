namespace MinIOExample.Infrastructure.Settings;

public record MinioSettings
{
    public const string SectionName = "Minio";
    public string Endpoint { get; init; }
    public string AccessKey { get; init; }
    public string SecretKey { get; init; }
    public string Bucket { get; init; }
}