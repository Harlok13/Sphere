using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Infra.Data.Configurations;

public class PlayerStatisticConfiguration : IEntityTypeConfiguration<PlayerStatistic>
{
    public void Configure(EntityTypeBuilder<PlayerStatistic> builder)
    {
        builder.HasKey(e => e.Id).HasName("player_statistics_pkey");

        builder.ToTable("player_statistics");

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(e => e.Matches)
            .HasColumnName("matches")
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(e => e.Wins)
            .HasColumnName("wins")
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(e => e.Loses)
            .HasColumnName("loses")
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(e => e.Draws)
            .HasColumnName("draws")
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(e => e.Level)
            .HasColumnName("level")
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(e => e.AllExp)
            .HasColumnName("all_exp")
            .HasDefaultValue(0)
            .IsRequired();
        
        builder.Property(e => e.CurrentExp)
            .HasColumnName("current_exp")
            .HasDefaultValue(0)
            .IsRequired();
        
        builder.Property(e => e.TargetExp)
            .HasColumnName("target_exp")
            .HasDefaultValue(100)
            .IsRequired();

        builder.Property(e => e.Money)
            .HasColumnName("money")
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(e => e.Likes)
            .HasColumnName("likes")
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(e => e.Has21)
            .HasColumnName("has21")
            .HasDefaultValue(0)
            .IsRequired();
        
        // builder.Property(e => e.CreatedAt)
        //     .HasColumnName("created_at")
        //     .HasDefaultValueSql("NOW()");
        // builder.Property(e => e.UpdatedAt)
        //     .HasColumnName("updated_at")
        //     .HasDefaultValueSql("NOW()");
    }
}