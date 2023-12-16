using App.Domain.Entities;

namespace App.Application.Repositories;

public interface IRoomRepository
{
    Task AddNewRoomAsync(Room room, CancellationToken cT);

    Task<Room?> GetRoomByIdAsync(Guid roomId, CancellationToken cT);

    Task<ICollection<Room>?> GetFirstPageAsync(CancellationToken cT);
    Task RemoveRoomAsync(Guid roomId, CancellationToken cT);
}