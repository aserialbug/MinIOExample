namespace MinIOExample.Application.Models;

public class UserId : BaseId
{
    // Пока нет авторизации
    public static UserId Admin => new UserId("83C8E1FF-7A49-4B10-9DAE-00A17DA34C6E");
    
    public UserId(string id) : base(id)
    {
    }
    
    public static bool operator ==(UserId a, UserId b) => EqualOperator(a, b);

    public static bool operator !=(UserId a, UserId b) => NotEqualOperator(a, b);
}