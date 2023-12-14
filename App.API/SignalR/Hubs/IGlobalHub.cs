using App.Contracts.Responses;
using App.Domain.Entities;

namespace Sphere.SignalR.Hubs;

public interface IGlobalHub
{
    Task ReceiveNewRoom(RoomInLobbyResponse roomInLobbyResponse, CancellationToken cT);

    Task ReceiveUpdatedRoom(RoomInLobbyResponse roomInLobbyResponse, CancellationToken cT);
    
    Task ReceiveNewPlayer(PlayerResponse playerResponse, CancellationToken cT);

    Task ReceiveOwnPlayerData(PlayerResponse playerResponse, RoomInLobbyResponse roomInLobbyResponse, IEnumerable<PlayerResponse> playerResponses, CancellationToken cT);

    Task ReceiveUpdatedPlayersList(IEnumerable<PlayerResponse> playersResponse, CancellationToken cT);

    Task Test(string message);
}