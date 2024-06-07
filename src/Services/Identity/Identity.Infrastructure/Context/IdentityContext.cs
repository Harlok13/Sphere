using Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Context;

public class IdentityContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public IdentityContext(DbContextOptions<IdentityContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.ApplyConfigurationsFromAssembly(typeof(IdentityContext).Assembly);
    }
}