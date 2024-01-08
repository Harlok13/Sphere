using App.Contracts.Data;
using App.Domain.Entities.RoomEntity;
using App.Domain.Primitives;
using App.Domain.Shared;

namespace App.Application.Repositories.RoomRepository;


public interface IRoomRepository
{
    Task AddAsync(Room room, CancellationToken cT);

    Task<Result<Room>> GetByIdAsync(Guid? roomId, CancellationToken cT);
    
    Task<Result<RoomDto>> GetByIdAsNoTrackingAsync(Guid? roomId, CancellationToken cT);

    Task<ICollection<Room>?> GetFirstPageAsNoTrackingAsync(CancellationToken cT);

    Task<Result> RemoveAsync(Guid? roomId, CancellationToken cT);

    Task<Result<Room>> GetByPlayerIdAsync1(Guid? playerId, CancellationToken cT);
    Task<Room?> GetByPlayerIdAsNoTrackingAsync2(Guid playerId, CancellationToken cT);

    Task<Result<RoomIdDto>> GetIdByPlayerIdAsync(Guid? playerId, CancellationToken cT);

    void Update(Room room);
}

