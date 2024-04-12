using App.Domain.Configurations;
using App.GrpcClient.Base;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UserInteractionClient;

namespace App.GrpcClient.UserInteractionClient;

public class UserInteractionClient : 
    BaseClient<UserInteraction.UserInteractionClient, UserInteractionClient>, 
    IUserInteractionClient
{
    public UserInteractionClient(
        ILogger<UserInteractionClient> logger,
        IOptions<GrpcConfiguration> grpcConfiguration) : 
        base (logger, grpcConfiguration.Value.GrpcUserInteractionServiceUrl) { }
    
    public Task<AddToFriendsResponse> AddToFriendsAsync(AddToFriendsRequest request)
        => Task.FromResult(Client.AddToFriends(request));

}