namespace Infrastructure.Exceptions.DbConnectionExceptions;

public class InvalidEnvironmentName : ApplicationException
{
    public InvalidEnvironmentName(string message) : base(message)
    {
        
    }
}