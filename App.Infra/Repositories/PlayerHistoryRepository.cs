using App.Application.Repositories;
using App.Domain.Entities;
using App.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace App.Infra.Repositories;

public class PlayerHistoryRepository : IPlayerHistoryRepository
{
    private readonly ApplicationContext _context;

    public PlayerHistoryRepository(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<ICollection<PlayerHistory>?> GetFirstFiveRecordsAsync(Guid playerId, CancellationToken cT)
    {
        return await _context.PlayerHistories
            .Where(e => e.PlayerId == playerId)
            .OrderByDescending(e => e.PlayedAt)
            .Take(5)
            .ToArrayAsync(cT);
    }
}