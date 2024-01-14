using App.Domain.Entities;

namespace App.Application.Repositories;

public interface IPlayerHistoryRepository
{
    Task<ICollection<PlayerHistory>?> GetFirstFiveRecordsAsNoTrackingAsync(Guid playerId, CancellationToken cT);
}