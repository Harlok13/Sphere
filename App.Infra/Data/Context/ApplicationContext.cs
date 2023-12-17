using App.Domain.Entities;
using App.Domain.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace App.Infra.Data.Context;

public class ApplicationContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    // public ApplicationContext() { }
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
    }

    public DbSet<Player> Players { get; set; } = null!;
    public DbSet<Room> Rooms { get; set; } = null!;
    public DbSet<PlayerHistory> PlayerHistories { get; set; } = null!;
    public DbSet<PlayerInfo> PlayerInfos { get; set; } = null!;
    public DbSet<Card> Cards { get; set; } = null!;
}