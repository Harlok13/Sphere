using Core.Shared;
using GameInteraction.Contracts.Data;
using GameInteraction.Domain.Entities.RoomEntity;

namespace GameInteraction.Application.Repositories;


public interface IRoomRepository
{
    Task AddAsync(Room room, CancellationToken cT);

    Task<Result<Room>> GetByIdAsync(Guid? roomId, CancellationToken cT);
    
    Task<Result<RoomDto>> GetByIdAsNoTrackingAsync(Guid? roomId, CancellationToken cT);

    Task<IEnumerable<RoomInLobbyDto>?> GetFirstPageAsNoTrackingAsync(CancellationToken cT);

    Task<Result> RemoveAsync(Guid? roomId, CancellationToken cT);

    Task<Result<Room>> GetByPlayerIdAsync(Guid? playerId, CancellationToken cT);
    
    Task<Result<RoomIdDto>> GetIdByPlayerIdAsNoTrackingAsync(Guid? playerId, CancellationToken cT);
}

