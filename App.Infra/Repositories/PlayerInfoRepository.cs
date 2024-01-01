using App.Application.Repositories;
using App.Domain.Shared;
using App.Domain.Shared.ResultImplementations;
using App.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using PlayerInfo = App.Domain.Entities.PlayerInfoEntity.PlayerInfo;

namespace App.Infra.Repositories;

public class PlayerInfoRepository : IPlayerInfoRepository
{
    private readonly ApplicationContext _context;

    public PlayerInfoRepository(ApplicationContext context)
    {
        _context = context;
    }
    public async Task CreatePlayerInfoAsync(Guid userId, string playerName, CancellationToken cT)
    {
        var playerInfo = PlayerInfo.Create(id: Guid.NewGuid(), userId: userId, playerName: playerName);
        await _context.PlayerInfos.AddAsync(playerInfo, cT);
    }

    public async Task<Result<PlayerInfo>> GetPlayerInfoByIdAsync(Guid? playerId, CancellationToken cT)
    {
        if (playerId is null)
            return InvalidResult<PlayerInfo>.Create(
                new Error(""));

        var playerInfo = await _context.PlayerInfos
            .SingleOrDefaultAsync(x => x.UserId == playerId, cT);
        
        if (playerInfo is null)
            return NotFoundResult<PlayerInfo>.Create(
                new Error(""));
        
        return SuccessResult<PlayerInfo>.Create(playerInfo);
    }

    public async Task<PlayerInfo?> GetPlayerInfoByIdAsNoTrackingAsync(Guid playerId, CancellationToken cT)
    {
        return await _context.PlayerInfos
            .AsNoTracking()
            .SingleOrDefaultAsync(e => e.UserId == playerId, cT);
    }

    public Task PlayerWinActionAsync(Guid userId, CancellationToken cT)
    {
        throw new NotImplementedException();
    }

    public Task PlayerLoseActionAsync(Guid userId, CancellationToken cT)
    {
        throw new NotImplementedException();
    }

    public Task PlayerDrawActionAsync(Guid userId, CancellationToken cT)
    {
        throw new NotImplementedException();
    }

    public Task PlayerHas21ActionAsync(Guid userId, CancellationToken cT)
    {
        throw new NotImplementedException();
    }
}
