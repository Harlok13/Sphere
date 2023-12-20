using App.Application.Repositories;
using App.Contracts.Identity.Responses;
using App.Domain.Entities;
using App.Infra.Data.Context;
using Microsoft.AspNetCore.Http.HttpResults;
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

    public async Task<PlayerInfo?> GetPlayerInfoByIdAsync(Guid playerId, CancellationToken cT)
    {
        return await _context.PlayerInfos.SingleOrDefaultAsync(x => x.UserId == playerId, cT);
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
