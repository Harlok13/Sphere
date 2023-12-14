using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Infra.Data.Configurations;

public class PlayerConfiguration : IEntityTypeConfiguration<Player>
{
    public void Configure(EntityTypeBuilder<Player> builder)
    {
        // builder.HasKey(e => e.Id).HasName("player_pkey");

        builder.ToTable("players");

        builder.Property(e => e.Id)
            .HasColumnName("id");

        builder.Property(e => e.Cards)
            .HasColumnName("cards");

        builder.Property(e => e.RoomId)
            .HasColumnName("room_id");

        builder.Property(e => e.Readiness)
            .HasColumnName("readiness")
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(e => e.Score)
            .HasColumnName("score")
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(e => e.PlayerName)
            .HasColumnName("player_name")
            .IsRequired()
            .HasMaxLength(25);

        builder.Property(e => e.AvatarUrl)
            .HasColumnName("avatar_url")
            .IsRequired()
            .HasDefaultValue("img/avatars/default_avatar.png");

        builder.Property(e => e.IsLeader)
            .HasColumnName("is_leader")
            .IsRequired()
            .HasDefaultValue(false);
    }
}