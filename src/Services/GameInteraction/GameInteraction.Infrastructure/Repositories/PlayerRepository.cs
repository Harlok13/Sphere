using Core.Shared;
using Core.Shared.ResultImplementations;
using GameInteraction.Application.Repositories;
using GameInteraction.Contracts.Data;
using GameInteraction.Contracts.Mapper;
using GameInteraction.Domain.Entities.PlayerEntity;
using GameInteraction.Infrastructure.Context;
using GameInteraction.Infrastructure.Messages;
using Microsoft.EntityFrameworkCore;

namespace GameInteraction.Infrastructure.Repositories;

public class PlayerRepository : IPlayerRepository
{
    private readonly GameInteractionContext _context;

    public PlayerRepository(GameInteractionContext context) => _context = context;

    public async Task<Result<PlayerDto>> GetPlayerByIdAsNoTrackingAsync(Guid? id, CancellationToken cT)
    {
        try
        {
            if (id is null) return InvalidResult<PlayerDto>.Create(
                new Error(ErrorMessages.Player.IdIsNull()));
            
            var playerDto = await _context.Set<Player>()
                .Where(p => p.Id == id)
                .Include(p => p.Room)
                .Select(p => PlayerMapper.MapPlayerToPlayerDto(p))
                .SingleOrDefaultAsync(cT);

            if (playerDto is null) return NotFoundResult<PlayerDto>.Create(
                new Error(ErrorMessages.Player.NotFound(id.ToString()!)));

            return SuccessResult<PlayerDto>.Create(playerDto);
        }
        catch (InvalidOperationException _)
        {
            return InvalidResult<PlayerDto>.Create(
                new Error(ErrorMessages.Player.ContainsMtOne(id.ToString()!)));
        }
        catch (ArgumentNullException _)
        {
            return InvalidResult<PlayerDto>.Create(
                new Error(ErrorMessages.Player.SourceIsNull()));
        }
        catch (OperationCanceledException _)
        {
            return InvalidResult<PlayerDto>.Create(
                new Error(ErrorMessages.Player.OperationCanceled()));
        }
        catch (Exception _)
        {
            return UnexpectedResult<PlayerDto>.Create();
        }
    }

    public bool CheckPlayerExists(Guid playerId)
        => _context.Set<Player>().Any(p => p.Id == playerId);
}