using Microsoft.EntityFrameworkCore;
using UserInteraction.Application.Repositories;
using UserInteraction.Contracts.Data;
using UserInteraction.Contracts.Mapper;
using UserInteraction.Domain.Entities;
using UserInteraction.Infrastructure.Context;

namespace UserInteraction.Infrastructure.Repositories;

public class PlayerHistoryRepository : IPlayerHistoryRepository
{
    private readonly UserInteractionContext _context;

    public PlayerHistoryRepository(UserInteractionContext context)
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