using App.Contracts.Data;
using Core.Shared;

namespace App.Application.Repositories;

public interface IPlayerRepository
{
    Task<Result<PlayerDto>> GetPlayerByIdAsNoTrackingAsync(Guid? id, CancellationToken cT);

    bool CheckPlayerExists(Guid playerId);
}