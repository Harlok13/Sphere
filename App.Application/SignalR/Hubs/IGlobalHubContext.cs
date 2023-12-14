using App.Domain.Entities;

namespace App.Application.SignalR.Hubs;

public interface IGlobalHubContext
{
    Task AllClients_ReceiveNewRoom(Room room, CancellationToken cT);

    Task AddToGroupAsync(Guid roomId, CancellationToken cT);

    Task Group_ReceiveUpdatedRoom(Room room, CancellationToken cT);

    Task AllClients_ReceiveUpdatedRoom(Room room, CancellationToken cT);
}