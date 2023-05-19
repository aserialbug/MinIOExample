using System.Collections.Generic;

namespace MinIOExample.Application.Settings;

public record UploadRestrictionsSettings
{
    public const string SectionName = "UploadRestrictions";
    public double? MaxFileSizeMiB { get; init; }
    public HashSet<string> AllowedFileTypes { get; init; }
}