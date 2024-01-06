// using App.Domain.Entities;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Metadata.Builders;
//
// namespace App.Infra.Data.Configurations;
//
// public class CardsDeckConfiguration : IEntityTypeConfiguration<CardsDeck>
// {
//     public void Configure(EntityTypeBuilder<CardsDeck> builder)
//     {
//         builder.HasKey(e => e.Id).HasName("cards_deck_pkey");
//
//         builder.ToTable("cards_deck");
//
//         builder.Property(e => e.Id)
//             .ValueGeneratedNever()
//             .HasColumnName("id")
//             .IsRequired();
//
//         builder
//             .HasOne(e => e.Room)
//             .WithOne(e => e.CardsDeck)
//             .OnDelete(DeleteBehavior.Cascade);
//     }
// }