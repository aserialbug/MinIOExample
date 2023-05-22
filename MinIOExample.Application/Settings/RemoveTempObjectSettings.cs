namespace MinIOExample.Application.Settings;

public record RemoveTempObjectSettings
{
    public const string SectionName = "RemoveTempObjects";
    
    public int? RemoveIntervalMinutes { get; init; }
}