using App.GrpcClient.Configurations;
using Core.Base;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using GrpcUserInteractionClient;

namespace App.GrpcClient.UserInteractionClient;

public class UserInteractionClient : 
    BaseGrpcClient<UserInteraction.UserInteractionClient, UserInteractionClient>, 
    IUserInteractionClient
{
    public UserInteractionClient(
        ILogger<UserInteractionClient> logger,
        IOptions<GrpcConfiguration> grpcConfiguration) : 
        base (logger, grpcConfiguration.Value.GrpcUserInteractionServiceUrl) { }
    
    public Task<GrpcAddToFriendsResponse> AddToFriendsAsync(GrpcAddToFriendsRequest request, CancellationToken cT)
        => Task.FromResult(Client.AddToFriends(request, cancellationToken: cT));

    public Task<GrpcCreatePlayerInfoResponse> CreatePlayerInfoAsync(GrpcCreatePlayerInfoRequest request, CancellationToken cT)
        => Task.FromResult(Client.CreatePlayerInfo(request, cancellationToken: cT));

    public Task<GrpcGetPlayerInfoResponse> GetPlayerInfoAsync(GrpcGetPlayerInfoRequest request, CancellationToken cT)
        => Task.FromResult(Client.GetPlayerInfo(request, cancellationToken: cT));
}