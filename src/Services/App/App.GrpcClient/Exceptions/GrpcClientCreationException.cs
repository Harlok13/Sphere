namespace App.GrpcClient.Exceptions;

public class GrpcClientCreationException : ApplicationException
{
    public GrpcClientCreationException(string message) : base(message) { }
}