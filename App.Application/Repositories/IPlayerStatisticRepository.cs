using App.Contracts.Identity.Responses;
using App.Domain.Entities;

namespace App.Application.Repositories;

public interface IPlayerStatisticRepository
{
    Task CreatePlayerStatisticAsync(Guid userId, CancellationToken cT);
    Task<PlayerStatistic?> GetPlayerStatisticAsync(Guid userId, CancellationToken cT);

    Task PlayerWinActionAsync(Guid userId, CancellationToken cT);
    Task PlayerLoseActionAsync(Guid userId, CancellationToken cT);
    Task PlayerDrawActionAsync(Guid userId, CancellationToken cT);
    Task PlayerHas21ActionAsync(Guid userId, CancellationToken cT);
}