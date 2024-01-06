using App.Application.Repositories;
using App.Contracts.Data;
using App.Domain.Shared;
using App.Domain.Shared.ResultImplementations;
using App.Infra.Data.Context;
using App.Infra.Messages;
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
                new Error(ErrorMessages.ArgumentIsNull(nameof(playerId), nameof(GetPlayerInfoByIdAsync))));

        var playerInfo = await _context.PlayerInfos
            .SingleOrDefaultAsync(x => x.UserId == playerId, cT);
        
        if (playerInfo is null)
            return NotFoundResult<PlayerInfo>.Create(
                new Error(ErrorMessages.NotFound(nameof(playerInfo), nameof(GetPlayerInfoByIdAsync))));
        
        return SuccessResult<PlayerInfo>.Create(playerInfo);
    }

    public async Task<PlayerInfo?> GetPlayerInfoByIdAsNoTrackingAsync(Guid playerId, CancellationToken cT)
    {
        return await _context.PlayerInfos
            .AsNoTracking()
            .SingleOrDefaultAsync(e => e.UserId == playerId, cT);
    }

    public async Task<Result<PlayerInfoMoneyDto>> GetMoneyByIdAsync(Guid? playerId, CancellationToken cT)
    {
        if (playerId is null)
            return InvalidResult<PlayerInfoMoneyDto>.Create(
                new Error(ErrorMessages.ArgumentIsNull(nameof(playerId), nameof(GetMoneyByIdAsync))));

        var money = await _context.Set<PlayerInfo>()
            .Where(p => p.UserId == playerId)
            .Select(p => new PlayerInfoMoneyDto(p.Money))
            .SingleOrDefaultAsync(cT);

        if (money is null)
            return NotFoundResult<PlayerInfoMoneyDto>.Create(
                new Error(ErrorMessages.NotFound(nameof(money), nameof(GetMoneyByIdAsync))));

        return SuccessResult<PlayerInfoMoneyDto>.Create(money);
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
