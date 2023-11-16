using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sphere.DAL.Models;

namespace Sphere.DAL.Configurations;

public class UserHistoryConfiguration : IEntityTypeConfiguration<UserHistoryModel>
{
    public void Configure(EntityTypeBuilder<UserHistoryModel> builder)
    {
        builder.HasKey(e => e.Id).HasName("users_history_pkey");

        builder.ToTable("user_history");

        builder.Property(e => e.Id)
            .HasColumnName("id");
        builder.Property(e => e.CardsPlayed)
            .HasColumnType("jsonb")
            .HasColumnName("cards_played");
        builder.Property(e => e.PlayedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("played_at");
        builder.Property(e => e.Result)
            .HasMaxLength(24)
            .HasColumnName("result");
        builder.Property(e => e.Score)
            .HasMaxLength(10)
            .HasColumnName("score");
        builder.Property(e => e.UserId)
            .HasColumnName("user_id");
        builder.Property(e => e.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("NOW()");
        builder.Property(e => e.UpdatedAt)
            .HasColumnName("updated_at")
            .HasDefaultValueSql("NOW()");
    }
}