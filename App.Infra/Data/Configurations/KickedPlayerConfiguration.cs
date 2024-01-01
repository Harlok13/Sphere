using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Infra.Data.Configurations;

public class KickedPlayerConfiguration : IEntityTypeConfiguration<KickedPlayer>
{
    public void Configure(EntityTypeBuilder<KickedPlayer> builder)
    {
        builder.HasKey(e => e.Id).HasName("kicked_players_pkey");

        builder.ToTable("kicked_players");

        // builder.Property(e => e.PlayerId)
        //     .HasColumnName("player_id")
        //     .IsRequired();

        builder.Property(e => e.PlayerName)
            .HasColumnName("player_name")
            .HasColumnType("VARCHAR")
            .HasMaxLength(25)
            .IsRequired();

        // builder.Property(e => e.WhoKickId)
        //     .HasColumnName("who_kick_id")
        //     .IsRequired();

        builder.Property(e => e.WhoKickName)
            .HasColumnName("who_kick_name")
            .HasColumnType("VARCHAR")
            .HasMaxLength(25)
            .IsRequired();

        builder
            .HasOne(e => e.Room)
            .WithMany(e => e.KickedPlayers)
            .HasForeignKey(e => e.RoomId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}