namespace Infrastructure.Exceptions.DbConnectionExceptions;

public class ProdDbConnectionStringIsNotSet : ApplicationException
{
    public ProdDbConnectionStringIsNotSet(string message) : base(message)
    {
        
    }
}