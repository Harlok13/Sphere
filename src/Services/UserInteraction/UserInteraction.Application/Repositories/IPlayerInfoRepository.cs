using Core.Shared;
using UserInteraction.Contracts.Data;
using UserInteraction.Domain.Entities.PlayerInfoEntity;

namespace UserInteraction.Application.Repositories;

public interface IPlayerInfoRepository
{
    Task CreatePlayerInfoAsync(Guid userId, string playerName, CancellationToken cT);
    Task<Result<PlayerInfo>> GetPlayerInfoByIdAsync(Guid? playerId, CancellationToken cT = default); 
    Task<Result<PlayerInfoDto>> GetPlayerInfoByIdAsNoTrackingAsync(Guid? playerId, CancellationToken cT);
    Task<Result<PlayerInfoMoneyDto>> GetMoneyByIdAsync(Guid? playerId, CancellationToken cT);
    Task<Result<PlayerInfo>> GetPlayerInfoWithFriendsAsync(Guid? playerId, CancellationToken cT);
}