using App.Domain.Entities.RoomEntity;

namespace App.Application.Repositories.RoomRepository;

public abstract class RoomRepositoryDecorator : IRoomRepository
{
    protected readonly IRoomRepository RoomRepository;

    protected RoomRepositoryDecorator(IRoomRepository roomRepository)
    {
        RoomRepository = roomRepository;
    }

    public abstract Task RemoveRoomAsync(Guid roomId, CancellationToken cT);

    public virtual async Task AddNewRoomAsync(Room room, CancellationToken cT)
    {
        await RoomRepository.AddNewRoomAsync(room, cT);
    }

    public virtual async Task<Room?> GetRoomByIdAsync(Guid roomId, CancellationToken cT)
    {
        return await RoomRepository.GetRoomByIdAsync(roomId, cT);
    }

    public virtual async Task<Room?> GetRoomByIdAsNoTrackingAsync(Guid roomId, CancellationToken cT)
    {
        return await RoomRepository.GetRoomByIdAsNoTrackingAsync(roomId, cT);
    }

    public virtual async Task<ICollection<Room>?> GetFirstPageAsNoTrackingAsync(CancellationToken cT)
    {
        return await RoomRepository.GetFirstPageAsNoTrackingAsync(cT);
    }
}