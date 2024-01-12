namespace Sphere.Exceptions.RedisExceptions;

public class RedisDsnException : ApplicationException
{
    public RedisDsnException(string message) : base(message)
    {
        
    }
}