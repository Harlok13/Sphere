using App.Domain.Entities;
using App.Domain.Identity.Entities;

namespace App.Application.Repositories;

public interface IPlayerRepository
{
    Task<Player?> GetPlayerByIdAsync(Guid id, CancellationToken cT);
    Task CreatePlayerAsync(Player player, CancellationToken cT);
    Task RemovePlayerAsync(Guid playerId, CancellationToken cT);
}