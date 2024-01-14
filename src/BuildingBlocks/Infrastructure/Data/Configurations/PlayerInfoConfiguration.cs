using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayerInfo = App.Domain.Entities.PlayerInfoEntity.PlayerInfo;

namespace App.Infra.Data.Configurations;

public class PlayerInfoConfiguration : IEntityTypeConfiguration<PlayerInfo>
{
    public void Configure(EntityTypeBuilder<PlayerInfo> builder)
    {
        builder.HasKey(e => e.Id).HasName("player_infos_pkey");

        builder.ToTable("player_infos");

        builder.Ignore(e => e.DomainEvents);

        builder.Property(e => e.Id)
            .ValueGeneratedNever()
            .HasColumnName("id")
            .IsRequired();

        builder.Property(e => e.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(e => e.AvatarUrl)
            .HasColumnName("avatar_url")
            .HasMaxLength(64)
            .HasColumnType("VARCHAR")
            .HasDefaultValueSql("'img/avatars/default_avatar.png'::text")
            .IsRequired();

        builder.Property(e => e.PlayerName)
            .HasColumnName("player_name")
            .HasColumnType("VARCHAR")
            .HasMaxLength(25)
            .IsRequired();

        builder.Property(e => e.UserId)
            .HasColumnName("user_id")
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
            .HasDefaultValue(1000)
            .IsRequired();

        builder.Property(e => e.Likes)
            .HasColumnName("likes")
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(e => e.Has21)
            .HasColumnName("has21")
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(e => e.HasGold21)
            .HasColumnName("has_gold21")
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