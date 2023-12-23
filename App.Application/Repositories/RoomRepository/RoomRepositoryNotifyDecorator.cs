using System.Diagnostics;
using App.Domain.Primitives;

namespace App.Application.Repositories.RoomRepository;

public class RoomRepositoryNotifyDecorator : RoomRepositoryDecorator
{
    public record RemoveRoomEventArgs(Guid RoomId) : IEventArgs;
    public event EventHandlerArgsAsync<RemoveRoomEventArgs>? NotifyRemoveRoom;

    public RoomRepositoryNotifyDecorator(IRoomRepository roomRepository) : base(roomRepository) { }
    
    public override async Task RemoveRoomAsync(Guid roomId, CancellationToken cT)
    {
        await RoomRepository.RemoveRoomAsync(roomId, cT);
        await NotifyRemoveRoom?.Invoke(new RemoveRoomEventArgs(roomId), cT)!;
    }
}