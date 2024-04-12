using App.Domain.Configurations;
using App.GrpcClient.Exceptions;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using static App.GrpcClient.Messages.MessageTextInfo;

namespace App.GrpcClient.Base;

public abstract class BaseClient<TClient, TClassName>
    where TClient : class
    where TClassName : class
{
    protected readonly TClient Client;
    protected ILogger<TClassName> Logger;

    protected BaseClient(ILogger<TClassName> logger, string grpcServiceUrl)
    {
        Logger = logger;

        GrpcChannel channel = GrpcChannel.ForAddress(grpcServiceUrl);

        string exMessage = string.Format(GrpcClientCreationEx, nameof(TClient));
        if (Activator.CreateInstance(typeof(TClient), channel) is not TClient client)
            throw new GrpcClientCreationException(exMessage);

        Client = client;
    }
}