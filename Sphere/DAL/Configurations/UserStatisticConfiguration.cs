using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sphere.DAL.Models;

namespace Sphere.DAL.Configurations;

public class UserStatisticConfiguration : IEntityTypeConfiguration<UserStatisticModel>
{
    public void Configure(EntityTypeBuilder<UserStatisticModel> builder)
    {
        builder.HasKey(e => e.Id).HasName("user_statistic_pkey");

        builder.ToTable("user_statistic");

        builder.Property(e => e.Id)
            .HasColumnName("id");
        builder.Property(e => e.Matches)
            .HasColumnName("matches")
            .HasDefaultValue(0);
        builder.Property(e => e.Wins)
            .HasColumnName("wins")
            .HasDefaultValue(0);
        builder.Property(e => e.Loses)
            .HasColumnName("loses")
            .HasDefaultValue(0);
        builder.Property(e => e.Draws)
            .HasColumnName("draws")
            .HasDefaultValue(0);
        builder.Property(e => e.Level)
            .HasColumnName("money")
            .HasDefaultValue(0);
        builder.Property(e => e.Exp)
            .HasColumnName("exp")
            .HasDefaultValue(0);
        builder.Property(e => e.Money)
            .HasColumnName("level")
            .HasDefaultValue(0);
        builder.Property(e => e.Likes)
            .HasColumnName("likes")
            .HasDefaultValue(0);
        builder.Property(e => e.Has21)
            .HasColumnName("has21")
            .HasDefaultValue(0);
        builder.Property(e => e.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("NOW()");
        builder.Property(e => e.UpdatedAt)
            .HasColumnName("updated_at")
            .HasDefaultValueSql("NOW()");
    }
}
