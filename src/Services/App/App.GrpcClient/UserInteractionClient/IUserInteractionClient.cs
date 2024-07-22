using GrpcUserInteractionClient;

namespace App.GrpcClient.UserInteractionClient;

public interface IUserInteractionClient
{
    Task<GrpcAddToFriendsResponse> AddToFriendsAsync(GrpcAddToFriendsRequest request, CancellationToken cT);

    Task<GrpcCreatePlayerInfoResponse> CreatePlayerInfoAsync(GrpcCreatePlayerInfoRequest request, CancellationToken cT);

    Task<GrpcGetPlayerInfoResponse> GetPlayerInfoAsync(GrpcGetPlayerInfoRequest request, CancellationToken cT);
}