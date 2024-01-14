/*
using App.Application.Repositories.RoomRepository;
using App.Domain.Entities.RoomEntity;
using App.Domain.Primitives;
using Microsoft.Extensions.Caching.Distributed;

namespace App.Infra.Repositories;

public class RoomRepositoryCacheDecorator : RoomRepositoryDecorator
{
    private readonly IDistributedCache _cache;

    public RoomRepositoryCacheDecorator(IRoomRepository roomRepository, IDistributedCache cache) 
        : base(roomRepository)
    {
        _cache = cache;
    }

    // public override event EventHandlerAsync<RemoveRoomEventArgs>? NotifyRemoveRoom;

    public override async Task AddAsync(Room room, CancellationToken cT)
    {
        await RoomRepository.AddAsync(room, cT);
    }

    public override async Task<Room?> GetByIdAsync(Guid roomId, CancellationToken cT)
    {
        return await RoomRepository.GetByIdAsync(roomId, cT);
    }

    public override async Task<ICollection<Room>?> GetFirstPageAsNoTrackingAsync(CancellationToken cT)
    {
        return await RoomRepository.GetFirstPageAsNoTrackingAsync(cT);
    }

    public override async Task RemoveAsync(Guid roomId, CancellationToken cT)
    {
        await RoomRepository.RemoveAsync(roomId, cT);
    }
}
*/