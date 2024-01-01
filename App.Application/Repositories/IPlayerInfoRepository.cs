using App.Domain.Shared;
using PlayerInfo = App.Domain.Entities.PlayerInfoEntity.PlayerInfo;

namespace App.Application.Repositories;

public interface IPlayerInfoRepository
{
    Task CreatePlayerInfoAsync(Guid userId, string playerName, CancellationToken cT);
    Task<Result<PlayerInfo>> GetPlayerInfoByIdAsync(Guid? playerId, CancellationToken cT = default);  // TODO: rename ...ByPlayerId
    Task<PlayerInfo?> GetPlayerInfoByIdAsNoTrackingAsync(Guid playerId, CancellationToken cT);

    Task PlayerWinActionAsync(Guid userId, CancellationToken cT);
    Task PlayerLoseActionAsync(Guid userId, CancellationToken cT);
    Task PlayerDrawActionAsync(Guid userId, CancellationToken cT);
    Task PlayerHas21ActionAsync(Guid userId, CancellationToken cT);
}