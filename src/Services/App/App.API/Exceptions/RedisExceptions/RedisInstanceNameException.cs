namespace App.API.Exceptions.RedisExceptions;

public class RedisInstanceNameException : ApplicationException
{
    public RedisInstanceNameException(string message) : base(message)
    {
        
    }
}