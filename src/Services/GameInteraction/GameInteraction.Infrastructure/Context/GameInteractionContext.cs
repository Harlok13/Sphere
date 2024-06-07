using GameInteraction.Domain.Entities;
using GameInteraction.Domain.Entities.PlayerEntity;
using GameInteraction.Domain.Entities.RoomEntity;
using Microsoft.EntityFrameworkCore;

namespace GameInteraction.Infrastructure.Context;

public class GameInteractionContext : DbContext
{
    public GameInteractionContext(DbContextOptions<GameInteractionContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IInfrastructureAssemblyMarker).Assembly);
    }

    public DbSet<Player> Players { get; set; } = null!;
    public DbSet<Room> Rooms { get; set; } = null!;
    public DbSet<KickedPlayer> KickedPlayers { get; set; } = null!;
}