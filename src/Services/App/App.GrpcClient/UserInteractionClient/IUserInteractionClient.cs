using UserInteractionClient;

namespace App.GrpcClient.UserInteractionClient;

public interface IUserInteractionClient
{
    Task<AddToFriendsResponse> AddToFriendsAsync(AddToFriendsRequest request);
}