namespace App.API.Exceptions.RedisExceptions;

public class RedisDsnException : ApplicationException
{
    public RedisDsnException(string message) : base(message)
    {
        
    }
}