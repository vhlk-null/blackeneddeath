using Library.Domain.Models;
using Library.Domain.ValueObjects.Ids;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Data.Configurations;

public class AlbumConfiguration : IEntityTypeConfiguration<Album>
{
    public void Configure(EntityTypeBuilder<Album> entity)
    {
        entity.ToTable("albums");

        entity.HasKey(e => e.Id);

        entity.Property(e => e.Id)
            .HasConversion(albumId => albumId.Value, dbId => AlbumId.Of(dbId));

        entity.Property(e => e.Title)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("title");

        entity.Property(e => e.CoverUrl)
            .HasMaxLength(500)
            .HasColumnName("cover_url");

        entity.Property(e => e.Type)
            .HasColumnName("type");

        entity.OwnsOne(e => e.AlbumRelease, ar =>
        {
            ar.Property(r => r.ReleaseYear).HasColumnName("release_year");
            ar.Property(r => r.Format).HasColumnName("format");
        });

        entity.OwnsOne(e => e.LabelInfo, li =>
        {
            li.Property(l => l.Name).HasMaxLength(200).HasColumnName("label_name");
        });
        entity.Navigation(e => e.LabelInfo).IsRequired(false);

        entity.Navigation(e => e.AlbumBands).UsePropertyAccessMode(PropertyAccessMode.Field);
        entity.Navigation(e => e.AlbumGenres).UsePropertyAccessMode(PropertyAccessMode.Field);
        entity.Navigation(e => e.AlbumCountries).UsePropertyAccessMode(PropertyAccessMode.Field);
        entity.Navigation(e => e.AlbumTracks).UsePropertyAccessMode(PropertyAccessMode.Field);
        entity.Navigation(e => e.StreamingLinks).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
