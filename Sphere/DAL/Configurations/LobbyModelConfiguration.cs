using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sphere.DAL.Models;

namespace Sphere.DAL.Configurations;

public class LobbyModelConfiguration : IEntityTypeConfiguration<LobbyModel>
{
    public void Configure(EntityTypeBuilder<LobbyModel> builder)
    {
        builder
            .HasKey(e => e.Id)
            .HasName("lobby_pkey");

        builder.ToTable("lobby");

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .UseIdentityAlwaysColumn();

        builder.Property(e => e.Name)
            .HasColumnName("name")
            .IsRequired();

        builder.Property(e => e.Bid)
            .HasColumnName("bid");

        builder.Property(e => e.ImgUrl)
            .HasColumnName("img_url")
            .IsRequired();

        builder.Property(e => e.StartBid)
            .HasColumnName("start_bid")
            .IsRequired();

        builder.Property(e => e.Size)
            .HasColumnName("size")
            .HasMaxLength(4);
    }
}