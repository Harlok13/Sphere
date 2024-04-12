namespace Infrastructure.Exceptions.DbConnectionExceptions;

public class DevDbConnectionStringIsNotSet : ApplicationException
{
    public DevDbConnectionStringIsNotSet(string message) : base(message)
    {
        
    }
}