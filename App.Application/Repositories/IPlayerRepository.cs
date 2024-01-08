using App.Contracts.Data;
using App.Domain.Shared;

namespace App.Application.Repositories;

public interface IPlayerRepository
{
    // Task<Player?> GetPlayerByIdAsync(Guid id, CancellationToken cT);
    Task<Result<PlayerDto>> GetPlayerByIdAsNoTrackingAsync(Guid? id, CancellationToken cT);

    bool CheckPlayerExists(Guid playerId);
    // Task CreatePlayerAsync(Player player, CancellationToken cT);
    // Task RemovePlayerAsync(Guid playerId, CancellationToken cT);
}