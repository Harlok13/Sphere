using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Infra.Data.Configurations;

public class CardConfiguration : IEntityTypeConfiguration<Card>
{
    public void Configure(EntityTypeBuilder<Card> builder)
    {
        builder.HasKey(e => e.Id).HasName("cards_pkey");
        
        builder.ToTable("cards");

        builder.Property(e => e.Id)
            .ValueGeneratedNever()
            .HasColumnName("id")
            .IsRequired();
        
        builder.Property(e => e.X)
            .HasColumnName("x")
            .IsRequired();

        builder.Property(e => e.Y)
            .HasColumnName("y")
            .IsRequired();

        builder.Property(e => e.Width)
            .HasColumnName("width")
            .IsRequired();

        builder.Property(e => e.Height)
            .HasColumnName("height")
            .IsRequired();

        builder.Property(e => e.Value)
            .HasColumnName("value")
            .IsRequired();

        builder.Property(e => e.SuitValue)
            .HasColumnName("suit_value")
            .HasColumnType("VARCHAR")
            .HasMaxLength(20)
            .IsRequired();

        builder
            .HasOne(e => e.Player)
            .WithMany(e => e.Cards)
            .HasForeignKey(e => e.PlayerId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_cards_player_player_id");
    }
}