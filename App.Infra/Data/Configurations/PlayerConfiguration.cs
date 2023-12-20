using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Player = App.Domain.Entities.PlayerEntity.Player;

namespace App.Infra.Data.Configurations;

public class PlayerConfiguration : IEntityTypeConfiguration<Player>
{
    public void Configure(EntityTypeBuilder<Player> builder)
    {
        builder.HasKey(e => e.Id).HasName("players_pkey");

        builder.ToTable("players");

        builder.Property(e => e.Id)
            .ValueGeneratedNever()
            .HasColumnName("id");
        
        builder.Property(e => e.RoomId)
            .HasColumnName("room_id")
            .IsRequired();

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
            .HasColumnType("VARCHAR")
            .IsRequired()
            .HasMaxLength(25);

        builder.Property(e => e.AvatarUrl)
            .HasColumnName("avatar_url")
            .HasColumnType("VARCHAR")
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(e => e.IsLeader)
            .HasColumnName("is_leader")
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(e => e.ConnectionId)
            .HasColumnName("connection_id")
            .IsRequired();

        builder.Property(e => e.InGame)
            .HasColumnName("in_game")
            .IsRequired()
            .HasDefaultValue(false);

        builder
            .HasOne(e => e.Room)
            .WithMany(e => e.Players)
            .HasForeignKey(e => e.RoomId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}