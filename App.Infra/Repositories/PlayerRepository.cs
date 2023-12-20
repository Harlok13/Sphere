using App.Application.Repositories;
using App.Domain.Entities;
using App.Domain.Identity.Entities;
using App.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Player = App.Domain.Entities.PlayerEntity.Player;

namespace App.Infra.Repositories;

public class PlayerRepository : IPlayerRepository
{
    private readonly ApplicationContext _context;

    public PlayerRepository(ApplicationContext context)
    {
        _context = context;
    }
    
    public async Task<Player?> GetPlayerByIdAsync(Guid id, CancellationToken cT)
    {
        return await _context.Set<Player>()
            // .Include(p => p.PlayerInfo)
            .SingleOrDefaultAsync(p => p.Id == id, cT);
    }

    public async Task CreatePlayerAsync(Player player, CancellationToken cT)
    {
        // var player = Player.Create(applicationUser.Id, applicationUser.UserName!);
        await _context.AddAsync(player, cT);
    }

    public async Task RemovePlayerAsync(Guid playerId, CancellationToken cT)
    {
        var player = await _context.Set<Player>().SingleOrDefaultAsync(p => p.Id == playerId, cT);
        if (player is not null) _context.Set<Player>().Remove(player);
    }
}