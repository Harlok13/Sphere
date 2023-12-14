using App.Application.Repositories;
using App.Contracts.Identity.Responses;
using App.Domain.Entities;
using App.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace App.Infra.Repositories;

public class PlayerStatisticRepository : IPlayerStatisticRepository
{
    private readonly ApplicationContext _context;

    public PlayerStatisticRepository(ApplicationContext context)
    {
        _context = context;
    }
    public async Task CreatePlayerStatisticAsync(Guid userId, CancellationToken cT)
    {
        var statistic = PlayerStatistic.Create(Guid.NewGuid(), userId);
        await _context.PlayerStatistics.AddAsync(statistic, cT);
    }

    public async Task<PlayerStatistic?> GetPlayerStatisticAsync(Guid userId, CancellationToken cT)
    {
        return await _context.PlayerStatistics.SingleOrDefaultAsync(x => x.PlayerId == userId, cT);
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
