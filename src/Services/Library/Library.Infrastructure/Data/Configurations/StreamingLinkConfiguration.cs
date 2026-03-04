using Library.Domain.Models;
using Library.Domain.ValueObjects.Ids;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Data.Configurations;

public class StreamingLinkConfiguration : IEntityTypeConfiguration<StreamingLink>
{
    public void Configure(EntityTypeBuilder<StreamingLink> entity)
    {
        entity.ToTable("streaming_links");

        entity.HasKey(e => e.Id);

        entity.Property(e => e.Id)
            .HasConversion(linkId => linkId.Value, dbId => StreamingLinkId.Of(dbId));

        entity.Property(e => e.Platform)
            .HasConversion<string>()
            .HasColumnName("platform");

        entity.Property(e => e.EmbedCode)
            .HasColumnType("text")
            .HasColumnName("embed_code");

        entity.Property<AlbumId>("AlbumId")
            .HasColumnName("album_id")
            .HasConversion(albumId => albumId.Value, dbId => AlbumId.Of(dbId));

        entity.HasOne<Album>()
            .WithMany(a => a.StreamingLinks)
            .HasForeignKey("AlbumId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
