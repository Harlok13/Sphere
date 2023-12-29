using App.Application.Repositories;
using App.Contracts.Data;
using App.Contracts.Mapper;
using App.Domain.Shared;
using App.Domain.Shared.ResultImplementations;
using App.Infra.Data.Context;
using App.Infra.Messages;
using Microsoft.EntityFrameworkCore;
using Player = App.Domain.Entities.PlayerEntity.Player;

namespace App.Infra.Repositories;

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
                .AsNoTracking()
                .Include(p => p.Room)
                .Where(p => p.Id == id)
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
}