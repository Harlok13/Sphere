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


    public RoomRepository(ApplicationContext context) => _context = context;

    public async Task AddAsync(Room room, CancellationToken cT)
        => await _context.AddAsync(room, cT);

    public void Update(Room room)  // TODO: redundant
    {
        _context.Set<Room>()
            .Update(room);
    }
    
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

    public Task<Result<RoomDto>> GetByIdAsNoTrackingAsync(Guid roomId, CancellationToken cT)
    {
        throw new NotImplementedException();
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
        // var roomDto = _context.Set<Room>()
        //     .AsNoTracking()
        //     .Where(r => r.Id == roomId)
        //     .Include(e => e.Players)
        //     .AsEnumerable()
        //     .Select(r => RoomMapper.MapRoomToRoomDto(r, PlayerMapper.MapManyPlayersToManyPlayersDto(r.Players)))
        //     .SingleOrDefault()!;
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

    public async Task<Result<Room>> GetByPlayerIdAsync1(Guid? playerId, CancellationToken cT)
    {
        if (playerId is null)
            return InvalidResult<Room>.Create(
                new Error(ErrorMessages.ArgumentIsNull(nameof(playerId), nameof(GetByPlayerIdAsync1)))); 

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
                new Error(ErrorMessages.NotFound(nameof(room), nameof(GetByPlayerIdAsync1))));

        return SuccessResult<Room>.Create(room);
    }

    [Obsolete]
    public async Task<Room?> GetByPlayerIdAsNoTrackingAsync2(Guid playerId, CancellationToken cT)
    {
        var players = _context.Set<Room>()
            .AsNoTracking()
            .Include(e => e.Players
                .OrderBy(p => p.Id))
            .Include(r => r.KickedPlayers)
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
                new Error(ErrorMessages.ArgumentIsNull(nameof(playerId), nameof(GetIdByPlayerIdAsync))));
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
                // new Error(ErrorMessages.Room.NotFound((Guid)playerId)));
                new Error(ErrorMessages.NotFound(nameof(roomId), nameof(GetIdByPlayerIdAsync))));
        }

        return SuccessResult<RoomIdDto>.Create(roomId);
    }
}