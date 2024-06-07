namespace Core.Exceptions;

public class GrpcClientCreationException : ApplicationException
{
    public GrpcClientCreationException(string message) : base(message) { }
}