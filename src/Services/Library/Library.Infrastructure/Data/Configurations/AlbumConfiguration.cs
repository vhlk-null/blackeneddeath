using Library.Domain.Models;
using Library.Domain.ValueObjects.Ids;
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

        entity.Property(e => e.Slug)
            .IsRequired()
            .HasMaxLength(220)
            .HasColumnName("slug");

        entity.HasIndex(e => e.Slug)
            .IsUnique()
            .HasDatabaseName("ix_albums_slug");

        entity.Property(e => e.CoverUrl)
            .HasMaxLength(500)
            .HasColumnName("cover_url");

        entity.Property(e => e.Type)
            .HasConversion<string>()
            .HasColumnName("type");

        entity.ComplexProperty(e => e.AlbumRelease, ar =>
        {
            ar.Property(r => r.ReleaseYear).HasColumnName("release_year");
            ar.Property(r => r.Format)
                .HasConversion<string>()
                .HasColumnName("format");
        });

        entity.Property(e => e.LabelId)
            .HasConversion(labelId => labelId!.Value, dbId => LabelId.Of(dbId))
            .HasColumnName("label_id")
            .IsRequired(false);

        entity.Navigation(e => e.AlbumBands).UsePropertyAccessMode(PropertyAccessMode.Field);
        entity.Navigation(e => e.AlbumGenres).UsePropertyAccessMode(PropertyAccessMode.Field);
        entity.Navigation(e => e.AlbumCountries).UsePropertyAccessMode(PropertyAccessMode.Field);
        entity.Navigation(e => e.AlbumTracks).UsePropertyAccessMode(PropertyAccessMode.Field);
        entity.Navigation(e => e.StreamingLinks).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
