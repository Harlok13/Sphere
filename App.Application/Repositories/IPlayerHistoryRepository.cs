using App.Domain.Entities;

namespace App.Application.Repositories;

public interface IPlayerHistoryRepository
{
    Task<ICollection<PlayerHistory>?> GetFirstFiveRecordsAsync(Guid playerId, CancellationToken cT);
}