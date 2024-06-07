using App.Application.Repositories;
using App.Contracts.Data;
using App.Contracts.Mapper;
using Core.Shared;
using Core.Shared.ResultImplementations;
using Infrastructure.Data.Context;
using Infrastructure.Messages;
using Microsoft.EntityFrameworkCore;
using Player = App.Domain.Entities.PlayerEntity.Player;

namespace Infrastructure.Repositories;

public class PlayerRepository : IPlayerRepository
{
    private readonly ApplicationContext _context;

    public PlayerRepository(ApplicationContext context) => _context = context;

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