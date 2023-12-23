using App.Domain.Entities.RoomEntity;
using App.Domain.Primitives;

namespace App.Application.Repositories.RoomRepository;


public interface IRoomRepository
{
    Task AddNewRoomAsync(Room room, CancellationToken cT);

    Task<Room?> GetRoomByIdAsync(Guid roomId, CancellationToken cT);
    
    Task<Room?> GetRoomByIdAsNoTrackingAsync(Guid roomId, CancellationToken cT);

    Task<ICollection<Room>?> GetFirstPageAsNoTrackingAsync(CancellationToken cT);

    Task RemoveRoomAsync(Guid roomId, CancellationToken cT);
}