using System.ComponentModel.DataAnnotations;

namespace MinIOExample.Commands;

public record AcceptToPermanentStorageCommand
{
    [Required]
    public string[] FileIds { get; init; }
}