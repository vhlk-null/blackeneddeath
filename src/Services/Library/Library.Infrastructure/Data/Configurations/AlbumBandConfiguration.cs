using Library.Domain.Models.JoinTables;
using Library.Domain.ValueObjects.Ids;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Data.Configurations;

public class AlbumBandConfiguration : IEntityTypeConfiguration<AlbumBand>
{
    public void Configure(EntityTypeBuilder<AlbumBand> entity)
    {
        entity.ToTable("album_bands");

        entity.HasKey(e => new { e.AlbumId, e.BandId });

        entity.Property(e => e.AlbumId)
            .HasConversion(id => id.Value, dbId => AlbumId.Of(dbId))
            .HasColumnName("album_id");

        entity.Property(e => e.BandId)
            .HasConversion(id => id.Value, dbId => BandId.Of(dbId))
            .HasColumnName("band_id");

        entity.Property(e => e.Order)
            .HasColumnName("order")
            .HasDefaultValue(0);
    }
}
