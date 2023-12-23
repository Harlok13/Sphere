using App.Application.Repositories.RoomRepository;
using App.Domain.Entities.RoomEntity;
using App.Domain.Primitives;
using App.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace App.Infra.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly ApplicationContext _context;
    
    // public event EventHandlerAsync<RemoveRoomEventArgs> NotifyRemoveRoom;

    public RoomRepository(ApplicationContext context)
    {
        _context = context;
    }

    public async Task AddNewRoomAsync(Room room, CancellationToken cT)
    {
        await _context.AddAsync(room, cT);
    }

    public async Task<Room?> GetRoomByIdAsync(Guid roomId, CancellationToken cT)
    {
        return await _context.Set<Room>()
            .Include(e => e.Players
                .OrderBy(p => p.Id))
            .SingleOrDefaultAsync(r => r.Id == roomId, cT);
    }

    public async Task<Room?> GetRoomByIdAsNoTrackingAsync(Guid roomId, CancellationToken cT)
    {
        return await _context.Set<Room>()
            .AsNoTracking()
            .Include(e => e.Players)
            .SingleOrDefaultAsync(r => r.Id == roomId, cT);
    }

    public async Task<ICollection<Room>?> GetFirstPageAsNoTrackingAsync(CancellationToken cT)
    {
        return await _context.Rooms
            .AsNoTracking()
            .Take(15)
            .Include(e => e.Players)
            .ToArrayAsync(cT); // TODO: 15 - const 
    }

    public async Task RemoveRoomAsync(Guid roomId, CancellationToken cT)
    {
        var room = await _context.Set<Room>()
            .Include(e => e.Players)
            .SingleOrDefaultAsync(e => e.Id == roomId, cT);

        _context.Set<Room>().Remove(room);
    }
}