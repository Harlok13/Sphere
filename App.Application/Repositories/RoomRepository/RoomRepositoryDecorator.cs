using App.Domain.Entities.RoomEntity;
using App.Domain.Primitives;

namespace App.Application.Repositories.RoomRepository;

public abstract class RoomRepositoryDecorator : IRoomRepository
{
    protected readonly IRoomRepository RoomRepository;

    protected RoomRepositoryDecorator(IRoomRepository roomRepository)
    {
        RoomRepository = roomRepository;
    }
    
    public virtual async Task AddNewRoomAsync(Room room, CancellationToken cT)
    {
        await RoomRepository.AddNewRoomAsync(room, cT);
    }

    public virtual async Task<Room?> GetRoomByIdAsync(Guid roomId, CancellationToken cT)
    {
        return await RoomRepository.GetRoomByIdAsync(roomId, cT);
    }

    public virtual async Task<ICollection<Room>?> GetFirstPageAsync(CancellationToken cT)
    {
        return await RoomRepository.GetFirstPageAsync(cT);
    }

    public abstract Task RemoveRoomAsync(Guid roomId, CancellationToken cT);
}
