using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Infra.Identity.Configurations;

public class AspNetRoles : IEntityTypeConfiguration<IdentityRole<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityRole<Guid>> builder)
    {
        builder.ToTable("asp_net_roles");

        builder.Property(e => e.Id)
            .HasColumnName("id");

        builder.Property(e => e.Name)
            .HasColumnName("name");

        builder.Property(e => e.NormalizedName)
            .HasColumnName("normalized_name");

        builder.Property(e => e.ConcurrencyStamp)
            .HasColumnName("concurrency_stamp");
        

        builder.HasData(new List<IdentityRole<Guid>>
        {
            new IdentityRole<Guid>()
            {
                Id = Guid.NewGuid(),
                Name = "Member",
                NormalizedName = "MEMBER",
                ConcurrencyStamp = null
            },
            new IdentityRole<Guid>()
            {
                Id = Guid.NewGuid(),
                Name = "Moderator",
                NormalizedName = "MODERATOR",
                ConcurrencyStamp = null
            },
            new IdentityRole<Guid>()
            {
                Id = Guid.NewGuid(),
                Name = "Administrator",
                NormalizedName = "ADMINISTRATOR",
                ConcurrencyStamp = null
            }
        });
    }
}