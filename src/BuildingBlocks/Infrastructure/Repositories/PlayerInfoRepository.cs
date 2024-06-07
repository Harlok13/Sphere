using App.Application.Repositories;
using App.Contracts.Data;
using App.Contracts.Mapper;
using Core.Shared;
using Core.Shared.ResultImplementations;
using Infrastructure.Data.Context;
using Infrastructure.Messages;
using Microsoft.EntityFrameworkCore;
using PlayerInfo = App.Domain.Entities.PlayerInfoEntity.PlayerInfo;

namespace Infrastructure.Repositories;

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
        await _context.Set<PlayerInfo>().AddAsync(playerInfo, cT);
    }

    public async Task<Result<PlayerInfo>> GetPlayerInfoByIdAsync(Guid? playerId, CancellationToken cT)
    {
        if (playerId is null)
            return InvalidResult<PlayerInfo>.Create(
                new Error(ErrorMessages.ArgumentIsNull(nameof(playerId), nameof(GetPlayerInfoByIdAsync))));

        var playerInfo = await _context.Set<PlayerInfo>()
            .SingleOrDefaultAsync(x => x.UserId == playerId, cT);
        
        if (playerInfo is null)
            return NotFoundResult<PlayerInfo>.Create(
                new Error(ErrorMessages.NotFound(nameof(playerInfo), nameof(GetPlayerInfoByIdAsync))));
        
        return SuccessResult<PlayerInfo>.Create(playerInfo);
    }

    public async Task<Result<PlayerInfoDto>> GetPlayerInfoByIdAsNoTrackingAsync(Guid? playerId, CancellationToken cT)
    {
        if (playerId is null)
            return InvalidResult<PlayerInfoDto>.Create(
                new Error(ErrorMessages.ArgumentIsNull(nameof(playerId), nameof(GetPlayerInfoByIdAsNoTrackingAsync))));
        
        var playerInfoDto = await _context.Set<PlayerInfo>()
            .Where(pI => pI.UserId == playerId)
            .Select(pI => PlayerMapper.MapPlayerInfoToPlayerInfoDto(pI))
            .SingleOrDefaultAsync(cancellationToken: cT);

        if (playerInfoDto is null)
            return NotFoundResult<PlayerInfoDto>.Create(
                new Error(ErrorMessages.NotFound(nameof(playerInfoDto), nameof(GetPlayerInfoByIdAsNoTrackingAsync))));

        return SuccessResult<PlayerInfoDto>.Create(playerInfoDto);
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

    public async Task<Result<PlayerInfo>> GetPlayerInfoWithFriendsAsync(Guid? playerId, CancellationToken cT)
    {
        if (playerId is null)
            return InvalidResult<PlayerInfo>.Create(
                new Error(ErrorMessages.ArgumentIsNull(nameof(playerId), nameof(GetPlayerInfoWithFriendsAsync))));
        
        var playerInfo = await _context.Set<PlayerInfo>()
            .Where(p => p.UserId == playerId)
            .Include(p => p.Friends)
            .SingleOrDefaultAsync(cT);

        if (playerInfo is null)
            return NotFoundResult<PlayerInfo>.Create(
                new Error(ErrorMessages.NotFound(nameof(playerInfo), nameof(GetPlayerInfoWithFriendsAsync))));

        return SuccessResult<PlayerInfo>.Create(playerInfo);
    }
}
