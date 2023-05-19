namespace MinIOExample.Application.Models;

public abstract class BaseId : ValueObject
{
    protected const char Separator = '_';
    
    private readonly string _id;

    protected BaseId(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentNullException(nameof(id));

        _id = id;
    }

    public override string ToString() => _id;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return _id;
    }
}