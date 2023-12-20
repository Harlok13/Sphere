using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Infra.Data.Configurations;

public class PlayerHistoryConfiguration : IEntityTypeConfiguration<PlayerHistory>
{
    public void Configure(EntityTypeBuilder<PlayerHistory> builder)
    {
        builder.HasKey(e => e.Id).HasName("player_histories_pkey");
        
        builder.ToTable("player_histories");

        builder.Property(e => e.Id)
            .ValueGeneratedNever()
            .HasColumnName("id")
            .IsRequired();

        builder.Property(e => e.CardsPlayed)
            // .HasColumnType("jsonb")  // TODO: text?
            .HasColumnName("cards_played")
            .IsRequired();

        builder.Property(e => e.PlayedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("played_at")
            .IsRequired();

        builder.Property(e => e.Result)
            .HasColumnName("result")
            .IsRequired();

        builder.Property(e => e.Score)
            .HasMaxLength(10)
            .HasColumnType("VARCHAR")
            .HasColumnName("score")
            .IsRequired();

        builder.Property(e => e.PlayerId)
            .HasColumnName("player_id")
            .IsRequired();
        // builder.Property(e => e.CreatedAt)
        //     .HasColumnName("created_at")
        //     .HasDefaultValueSql("NOW()");
        // builder.Property(e => e.UpdatedAt)
        //     .HasColumnName("updated_at")
        //     .HasDefaultValueSql("NOW()");
    }
}