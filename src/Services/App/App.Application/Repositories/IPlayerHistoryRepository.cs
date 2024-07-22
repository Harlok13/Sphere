using App.Contracts.Data;

namespace App.Application.Repositories;

public interface IPlayerHistoryRepository
{
    Task<IEnumerable<PlayerHistoryDto>?> GetFirstFiveRecordsAsNoTrackingAsync(Guid playerId, CancellationToken cT);
}