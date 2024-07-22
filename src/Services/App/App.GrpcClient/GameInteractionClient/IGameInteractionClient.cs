using GrpcGameInteractionClient;

namespace App.GrpcClient.GameInteractionClient;

public interface IGameInteractionClient
{
    Task<GrpcCreateRoomResponse> CreateRoomAsync(GrpcCreateRoomRequest request, CancellationToken cT);
    
    Task<GrpcJoinToRoomResponse> JoinToRoomAsync(GrpcJoinToRoomRequest request, CancellationToken cT);
}