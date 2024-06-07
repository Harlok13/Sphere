using Core.Exceptions;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using static Core.Messages.MessageTextInfo;

namespace Core.Base;

public abstract class BaseGrpcClient<TClient, TClassName>
    where TClient : class
    where TClassName : class
{
    protected readonly TClient Client;
    protected ILogger<TClassName> Logger;

    protected BaseGrpcClient(ILogger<TClassName> logger, string grpcServiceUrl)
    {
        Logger = logger;

        GrpcChannel channel = GrpcChannel.ForAddress(grpcServiceUrl);

        string exMessage = string.Format(GrpcClientCreationEx, nameof(TClient));
        if (Activator.CreateInstance(typeof(TClient), channel) is not TClient client)
            throw new GrpcClientCreationException(exMessage);

        Client = client;
    }
}