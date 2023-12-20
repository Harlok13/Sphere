using App.Domain.Primitives;

namespace App.Domain.Entities.RoomEntity;

public sealed partial class Room
{
    public delegate Task PlayersHandlerAsync(Room sender, CancellationToken cT);

    public event EventHandlerAsync<Room>? NotifyPlayers;
    
    
    // public delegate Task StatusHandler
}