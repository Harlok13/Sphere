using Core.Shared;
using Core.Shared.ResultImplementations;
using GameInteraction.Application.Repositories;
using GameInteraction.Contracts.Data;
using GameInteraction.Contracts.Mapper;
using GameInteraction.Domain.Entities.PlayerEntity;
using GameInteraction.Domain.Entities.RoomEntity;
using GameInteraction.Infrastructure.Context;
using GameInteraction.Infrastructure.Messages;
using Microsoft.EntityFrameworkCore;

namespace GameInteraction.Infrastructure.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly GameInteractionContext _context;
    
    public RoomRepository(GameInteractionContext context) => _context = context;

    public async Task AddAsync(Room room, CancellationToken cT)
        => await _context.AddAsync(room, cT);
    
    
    public async Task<Result<Room>> GetByIdAsync(Guid? roomId, CancellationToken cT)
    {
        if (roomId is null)
            return InvalidResult<Room>.Create(
                new Error(ErrorMessages.ArgumentIsNull(nameof(roomId), nameof(GetByIdAsync))));

        var room = await _context.Set<Room>()
            .Where(r => r.Id == roomId)
            .Include(e => e.Players
                .OrderBy(p => p.Id))
            .Include(r => r.KickedPlayers)
            .SingleOrDefaultAsync(cT);

        if (room is null)
            return NotFoundResult<Room>.Create(
                new Error(ErrorMessages.NotFound(nameof(room), nameof(GetByIdAsync))));

        return SuccessResult<Room>.Create(room);
    }

    public async Task<Result<RoomDto>> GetByIdAsNoTrackingAsync(Guid? roomId, CancellationToken cT)
    {
        if (roomId is null)
            return InvalidResult<RoomDto>.Create(
                new Error(ErrorMessages.ArgumentIsNull(nameof(roomId), nameof(GetByIdAsync))));
        
        var room = await _context.Set<Room>()
            .AsNoTracking()
            .Include(e => e.Players
                .OrderBy(p => p.Id))
            .Include(r => r.KickedPlayers)
            .SingleOrDefaultAsync(r => r.Id == roomId, cT);
        
        if (room is null)
            return NotFoundResult<RoomDto>.Create(
                new Error(ErrorMessages.NotFound(nameof(room), nameof(GetByIdAsync))));

        return SuccessResult<RoomDto>.Create(
            RoomMapper.MapRoomToRoomDto(room, PlayerMapper.MapManyPlayersToManyPlayersDto(room.Players)));
    }

    public async Task<IEnumerable<RoomInLobbyDto>?> GetFirstPageAsNoTrackingAsync(CancellationToken cT)
    {
        var rooms = await _context.Set<Room>()
            .AsNoTracking()
            .OrderByDescending(x => x.Status)  // TODO: use created at field
            .Take(15)  // TODO: 15 - const 
            .Include(e => e.Players)
            .ToArrayAsync(cT);

        if (rooms is null) return null;

        return RoomMapper.MapManyRoomsToManyRoomsInLobbyDto(rooms);
    }

    public async Task<Result> RemoveAsync(Guid? roomId, CancellationToken cT) // TODO: fix
    {
        if (roomId is null)
            return Result.Create(
                isSuccess: false,
                error: new Error(ErrorMessages.ArgumentIsNull(nameof(roomId), nameof(RemoveAsync))));

        var room = await _context.Set<Room>()
            .SingleOrDefaultAsync(e => e.Id == roomId, cT);

        if (room is null)
            return Result.Create(
                isSuccess: false,
                error: new Error(ErrorMessages.NotFound(nameof(room), nameof(RemoveAsync))));

        _context.Set<Room>().Remove(room);

        return Result.CreateSuccess();
    }

    public async Task<Result<Room>> GetByPlayerIdAsync(Guid? playerId, CancellationToken cT)
    {
        if (playerId is null)
            return InvalidResult<Room>.Create(
                new Error(ErrorMessages.ArgumentIsNull(nameof(playerId), nameof(GetByPlayerIdAsync)))); 

        var room = await _context.Set<Room>()
            .Include(r => r.Players
                .OrderBy(p => p.Id))
            .Include(r => r.KickedPlayers)
            .Join(_context.Set<Player>(),
                r => r.Id,
                p => p.RoomId,
                (r, p) => new { Room = r, Player = p })
            .Where(rp => rp.Player.Id == playerId)
            .Select(rp => rp.Room)
            .SingleOrDefaultAsync(cT);
        
        if (room is null)
            return NotFoundResult<Room>.Create(
                new Error(ErrorMessages.NotFound(nameof(room), nameof(GetByPlayerIdAsync))));

        return SuccessResult<Room>.Create(room);
    }

    public async Task<Result<RoomIdDto>> GetIdByPlayerIdAsNoTrackingAsync(Guid? playerId, CancellationToken cT)
    {
        if (playerId is null)
            return InvalidResult<RoomIdDto>.Create(
                new Error(ErrorMessages.ArgumentIsNull(nameof(playerId), nameof(GetIdByPlayerIdAsNoTrackingAsync))));

        var roomId = await _context.Set<Room>()
            .Join(_context.Set<Player>(),
                r => r.Id,
                p => p.RoomId,
                (r, p) => new { Room = r, Player = p })
            .Where(rp => rp.Player.Id == playerId)
            .Select(rp => new RoomIdDto(rp.Room.Id))
            .SingleOrDefaultAsync(cT);

        if (roomId is null)
            return NotFoundResult<RoomIdDto>.Create(
                new Error(ErrorMessages.NotFound(nameof(roomId), nameof(GetIdByPlayerIdAsNoTrackingAsync))));

        return SuccessResult<RoomIdDto>.Create(roomId);
    }
}