using Library.Domain.Models;
using Library.Domain.Models.JoinTables;
using Library.Domain.ValueObjects.Ids;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Data.Configurations;

public class AlbumBandConfiguration : IEntityTypeConfiguration<AlbumBand>
{
    public void Configure(EntityTypeBuilder<AlbumBand> entity)
    {
        entity.ToTable("album_bands");

        entity.HasKey(e => new { e.AlbumId, e.BandId });

        entity.Property(e => e.AlbumId)
            .HasColumnName("album_id")
            .HasConversion(albumId => albumId.Value, dbId => AlbumId.Of(dbId));

        entity.Property(e => e.BandId)
            .HasColumnName("band_id")
            .HasConversion(bandId => bandId.Value, dbId => BandId.Of(dbId));

        entity.HasOne<Album>()
            .WithMany(a => a.AlbumBands)
            .HasForeignKey(e => e.AlbumId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne<Band>()
            .WithMany()
            .HasForeignKey(e => e.BandId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
