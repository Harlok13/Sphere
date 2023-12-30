using App.Application.Repositories.RoomRepository;
using App.Contracts.Data;
using App.Contracts.Mapper;
using App.Domain.Entities.PlayerEntity;
using App.Domain.Entities.RoomEntity;
using App.Domain.Shared;
using App.Domain.Shared.ResultImplementations;
using App.Infra.Data.Context;
using App.Infra.Messages;
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

    public async Task AddAsync(Room room, CancellationToken cT)
    {
        await _context.AddAsync(room, cT);
    }

    public async Task<Room?> GetByIdAsync(Guid roomId, CancellationToken cT)
    {
        return await _context.Set<Room>()
            .Where(r => r.Id == roomId)
            .Include(e => e.Players
                .OrderBy(p => p.Id))
            .SingleOrDefaultAsync(cT);
    }

    public async Task<Room?> GetByIdAsNoTrackingAsync(Guid roomId, CancellationToken cT)
    {
        return await _context.Set<Room>()
            .AsNoTracking()
            .Include(e => e.Players
                .OrderBy(p => p.Id))
            .SingleOrDefaultAsync(r => r.Id == roomId, cT);
        var roomDto = _context.Set<Room>()
            .AsNoTracking()
            .Where(r => r.Id == roomId)
            .Include(e => e.Players)
            .AsEnumerable()
            .Select(r => RoomMapper.MapRoomToRoomDto(r, PlayerMapper.MapManyPlayersToManyPlayersDto(r.Players)))
            .SingleOrDefault()!;
        // var roomDto = await _context.Set<Room>()
        //     .AsNoTracking()
        //     .Where(r => r.Id == roomId)
        //     .Include(e => e.Players)
        //     .Select(r => RoomMapper.MapRoomToRoomDto(r))
        //     .SingleOrDefaultAsync(cT);
    }

    public async Task<ICollection<Room>?> GetFirstPageAsNoTrackingAsync(CancellationToken cT)
    {
        return await _context.Rooms
            .AsNoTracking()
            .Take(15)
            .Include(e => e.Players)
            .ToArrayAsync(cT); // TODO: 15 - const 
    }

    public async Task RemoveAsync(Guid roomId, CancellationToken cT)
    {
        var room = await _context.Set<Room>()
            .Include(e => e.Players)  // TODO: redundant?
            .SingleOrDefaultAsync(e => e.Id == roomId, cT);

        _context.Set<Room>().Remove(room);
    }

    public async Task<Room?> GetByPlayerIdAsync1(Guid playerId, CancellationToken cT)
    {
        return await _context.Set<Room>()
            .Include(r => r.Players
                .OrderBy(p => p.Id))
            .Join(_context.Set<Player>(),
                r => r.Id,
                p => p.RoomId,
                (r, p) => new { Room = r, Player = p })
            .Where(rp => rp.Player.Id == playerId)
            .Select(rp => rp.Room)
            .SingleOrDefaultAsync(cT);
    }
    
    [Obsolete]
    public async Task<Room?> GetByPlayerIdAsNoTrackingAsync2(Guid playerId, CancellationToken cT)
    {
        var players = _context.Set<Room>()
            .AsNoTracking()
            .Include(e => e.Players
                .OrderBy(p => p.Id))
            .SelectMany(r => r.Players);
        
        var roomId = await players
            .Where(p => p.Id == playerId)
            .Select(p => p.RoomId).SingleOrDefaultAsync(cT);
        
        var room = await _context.Set<Room>()
            .SingleOrDefaultAsync(r => r.Id == roomId, cT);

        return room;
    }

    public async Task<Result<RoomIdDto>> GetIdByPlayerIdAsync(Guid? playerId, CancellationToken cT)
    {
        if (playerId is null)
        {
            return InvalidResult<RoomIdDto>.Create(
                new Error(ErrorMessages.Room.IdIsNull()));
        }
        
        var roomId = await _context.Set<Room>()
            .Join(_context.Set<Player>(),
                r => r.Id,
                p => p.RoomId,
                (r, p) => new { Room = r, Player = p })
            .Where(rp => rp.Player.Id == playerId)
            .Select(rp => new RoomIdDto(rp.Room.Id))
            .SingleOrDefaultAsync(cT);

        if (roomId is null)
        {
            return NotFoundResult<RoomIdDto>.Create(
                new Error(ErrorMessages.Room.NotFound((Guid)playerId)));
        }
        
        return SuccessResult<RoomIdDto>.Create(roomId);
    }
}

