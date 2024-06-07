using Microsoft.EntityFrameworkCore;
using UserInteraction.Domain.Entities;
using UserInteraction.Domain.Entities.PlayerInfoEntity;

namespace UserInteraction.Infrastructure.Context;

public class UserInteractionContext : DbContext
{
    public UserInteractionContext(DbContextOptions<UserInteractionContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IInfrastructureAssemblyMarker).Assembly);
    }

    public DbSet<PlayerHistory> PlayerHistories { get; set; } = null!;
    public DbSet<PlayerInfo> PlayerInfos { get; set; } = null!;

    // public DbSet<Friends> Friends { get; set; } = null!;
}