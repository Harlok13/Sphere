using Core.Shared;
using GameInteraction.Contracts.Data;

namespace GameInteraction.Application.Repositories;

public interface IPlayerRepository
{
    Task<Result<PlayerDto>> GetPlayerByIdAsNoTrackingAsync(Guid? id, CancellationToken cT);

    bool CheckPlayerExists(Guid playerId);
}