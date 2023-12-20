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

    public override async Task AddNewRoomAsync(Room room, CancellationToken cT)
    {
        await RoomRepository.AddNewRoomAsync(room, cT);
    }

    public override async Task<Room?> GetRoomByIdAsync(Guid roomId, CancellationToken cT)
    {
        return await RoomRepository.GetRoomByIdAsync(roomId, cT);
    }

    public override async Task<ICollection<Room>?> GetFirstPageAsync(CancellationToken cT)
    {
        return await RoomRepository.GetFirstPageAsync(cT);
    }

    public override async Task RemoveRoomAsync(Guid roomId, CancellationToken cT)
    {
        await RoomRepository.RemoveRoomAsync(roomId, cT);
    }
}