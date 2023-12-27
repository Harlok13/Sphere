using PlayerInfo = App.Domain.Entities.PlayerInfoEntity.PlayerInfo;

namespace App.Application.Repositories;

public interface IPlayerInfoRepository
{
    Task CreatePlayerInfoAsync(Guid userId, string playerName, CancellationToken cT);
    Task<PlayerInfo?> GetPlayerInfoByIdAsync(Guid playerId, CancellationToken cT = default);
    Task<PlayerInfo?> GetPlayerInfoByIdAsNoTrackingAsync(Guid playerId, CancellationToken cT);

    Task PlayerWinActionAsync(Guid userId, CancellationToken cT);
    Task PlayerLoseActionAsync(Guid userId, CancellationToken cT);
    Task PlayerDrawActionAsync(Guid userId, CancellationToken cT);
    Task PlayerHas21ActionAsync(Guid userId, CancellationToken cT);
}