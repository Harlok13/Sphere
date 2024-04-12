using App.Contracts.Data;
using App.Domain.Shared;

namespace App.Application.Repositories;

public interface IPlayerHistoryRepository
{
    Task<IEnumerable<PlayerHistoryDto>?> GetFirstFiveRecordsAsNoTrackingAsync(Guid playerId, CancellationToken cT);
}