using App.Domain.Entities;
using App.Domain.Entities.RoomEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Infra.Data.Configurations;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.HasKey(e => e.Id).HasName("rooms_pkey");

        builder.ToTable("rooms");

        builder.Ignore(e => e.DomainEvents);

        builder.Property(e => e.Id)
            .ValueGeneratedNever()
            .HasColumnName("id")
            .IsRequired();

        builder.Property(e => e.RoomSize)
            .HasColumnName("room_size")
            .HasMaxLength(4)
            .IsRequired();

        builder.Property(e => e.RoomName)
            .HasColumnName("room_name")
            .HasColumnType("VARCHAR")
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
            .HasColumnType("VARCHAR")
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(e => e.PlayersInRoom)
            .HasColumnName("players_in_room")
            .IsRequired();

        builder.Property(e => e.Status)
            .HasColumnName("status")
            .IsRequired();

        builder.Property(e => e.Bank)
            .HasColumnName("bank");

        builder.Property(e => e.LowerStartMoneyBound)
            .HasColumnName("lower_start_money_bound")
            .IsRequired();

        builder.Property(e => e.UpperStartMoneyBound)
            .HasColumnName("upper_start_money_bound")
            .IsRequired();

        builder.Property(e => e.CardsDeck)
            .HasColumnName("cards_deck")
            .HasColumnType("jsonb");

        builder.Property(e => e.GameHistory)
            .HasColumnName("game_history")
            .HasColumnType("jsonb");
    }
}