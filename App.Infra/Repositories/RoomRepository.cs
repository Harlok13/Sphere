using App.Application.Repositories;
using App.Domain.Entities;
using App.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace App.Infra.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly ApplicationContext _context;

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
            .Include(e => e.Players)
            .SingleOrDefaultAsync(r => r.Id == roomId, cT);
    }

    public async Task<ICollection<Room>?> GetFirstPageAsync(CancellationToken cT)
    {
        return await _context.Rooms
            .Take(15)
            .Include(e => e.Players)
            .ToArrayAsync(cT);  // TODO: 15 - const 
    }
}