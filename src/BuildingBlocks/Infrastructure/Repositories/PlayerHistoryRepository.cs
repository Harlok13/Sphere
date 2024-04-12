using App.Application.Repositories;
using App.Contracts.Data;
using App.Contracts.Mapper;
using App.Domain.Entities;
using Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class PlayerHistoryRepository : IPlayerHistoryRepository
{
    private readonly ApplicationContext _context;

    public PlayerHistoryRepository(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PlayerHistoryDto>?> GetFirstFiveRecordsAsNoTrackingAsync(Guid playerId, CancellationToken cT)
    {
        var playerHistories = await _context.Set<PlayerHistory>()
            .AsNoTracking()
            .Where(e => e.PlayerId == playerId)
            .OrderByDescending(e => e.PlayedAt)
            .Take(5)
            .ToArrayAsync(cT);

        if (playerHistories is null) return null;

        return PlayerMapper.MapManyPlayerHistoryToManyPlayerHistoryDtos(playerHistories);
    }
}