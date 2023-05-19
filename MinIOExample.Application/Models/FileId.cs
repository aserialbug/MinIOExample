using System;
using System.Linq;

namespace MinIOExample.Application.Models;

public class FileId : BaseId
{
    private FileId(string id) : base(id)
    {
    }

    public static FileId New() => new (string.Join(Separator, nameof(FileId), Guid.NewGuid().ToString("N")));

    public static FileId Parse(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(nameof(value));

        var sections = value.Split(Separator);
        if (sections.Length != 2 || 
            sections.First() != nameof(FileId) || 
            !Guid.TryParse(sections.Last(), out _))
            throw new ArgumentException($"Invalid value={value} for FileId");

        return new(value);
    }

    public static bool operator ==(FileId a, FileId b) => EqualOperator(a, b);

    public static bool operator !=(FileId a, FileId b) => NotEqualOperator(a, b);
}