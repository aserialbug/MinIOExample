namespace MinIOExample.Application.Exceptions;

public class PolicyViolationException : Exception
{
    public PolicyViolationException(string message) : base(message)
    {
        
    }
}