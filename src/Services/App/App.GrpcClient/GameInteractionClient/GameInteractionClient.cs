using App.GrpcClient.Configurations;
using Core.Base;
using GrpcGameInteractionClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace App.GrpcClient.GameInteractionClient;

public class GameInteractionClient :
    BaseGrpcClient<GameInteraction.GameInteractionClient, GameInteractionClient>,
    IGameInteractionClient
{
    public GameInteractionClient(
        ILogger<GameInteractionClient> logger,
        IOptions<GrpcConfiguration> grpcConfiguration) :
        base(logger, grpcConfiguration.Value.GrpcGameInteractionServiceUrl) { }

    public Task<GrpcCreateRoomResponse> CreateRoomAsync(GrpcCreateRoomRequest request, CancellationToken cT) =>
        Task.FromResult(Client.CreateRoom(request, cancellationToken: cT));

    public Task<GrpcJoinToRoomResponse> JoinToRoomAsync(GrpcJoinToRoomRequest request, CancellationToken cT) =>
        Task.FromResult(Client.JointToRoom(request, cancellationToken: cT));
}