namespace MinIOExample.Application.Settings;

public record RemoveTempObjectSettings
{

    public const string SectionName = "RemoveTempObjects";
    
    public int? RemoveTaskIntervalMinutes { get; init; }
    public int TempFileLifetimeMinutes { get; init; }
}