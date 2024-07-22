namespace App.API.Exceptions.CorsExceptions;

public class CorsPolicyException : ApplicationException
{
    public CorsPolicyException(string message) : base(message)
    {
        
    }
}