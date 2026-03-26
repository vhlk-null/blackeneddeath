using Library.Domain.Models;
using Library.Domain.Models.JoinTables;
using Library.Domain.ValueObjects.Ids;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Data.Configurations;

public class AlbumTagConfiguration : IEntityTypeConfiguration<AlbumTag>
{
    public void Configure(EntityTypeBuilder<AlbumTag> entity)
    {
        entity.ToTable("album_tags");

        entity.HasKey(e => new { e.AlbumId, e.TagId });

        entity.Property(e => e.AlbumId)
            .HasColumnName("album_id")
            .HasConversion(id => id.Value, dbId => AlbumId.Of(dbId));

        entity.Property(e => e.TagId)
            .HasColumnName("tag_id")
            .HasConversion(id => id.Value, dbId => TagId.Of(dbId));

        entity.HasOne<Album>()
            .WithMany(a => a.AlbumTags)
            .HasForeignKey(e => e.AlbumId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne<Tag>()
            .WithMany()
            .HasForeignKey(e => e.TagId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
