using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Infra.Data.Configurations;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.HasKey(e => e.Id).HasName("room_pkey");

        builder.ToTable("rooms");
            // .HasMany(e => e.Players)
            // .WithOne();
            // .HasForeignKey(e => e.Id);  // TODO how to fix this?

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(e => e.RoomSize)
            .HasColumnName("room_size")
            .HasMaxLength(4)
            .IsRequired();

        builder.Property(e => e.RoomName)
            .HasColumnName("room_name")
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(e => e.StartBid)
            .HasColumnName("start_bid")
            .IsRequired();

        builder.Property(e => e.MinBid)
            .HasColumnName("min_bid")
            .IsRequired();

        builder.Property(e => e.MaxBid)
            .HasColumnName("max_bid")
            .IsRequired();

        builder.Property(e => e.AvatarUrl)
            .HasColumnName("avatar_url")
            .IsRequired();

        builder.Property(e => e.PlayersInRoom)
            .HasColumnName("players_in_room")
            .IsRequired();

        builder.Property(e => e.Status)
            .HasColumnName("status")
            .IsRequired();

        // builder.Property(e => e.Players)
        //     .HasColumnName("players");
    }
}