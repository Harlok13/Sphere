using UserInteraction.Contracts.Data;

namespace UserInteraction.Application.Repositories;

public interface IPlayerHistoryRepository
{
    Task<IEnumerable<PlayerHistoryDto>?> GetFirstFiveRecordsAsNoTrackingAsync(Guid playerId, CancellationToken cT);
}