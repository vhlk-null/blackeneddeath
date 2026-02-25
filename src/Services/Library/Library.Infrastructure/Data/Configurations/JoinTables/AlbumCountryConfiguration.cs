using Library.Domain.Models;
using Library.Domain.Models.JoinTables;
using Library.Domain.ValueObjects.Ids;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Data.Configurations.JoinTables;

public class AlbumCountryConfiguration : IEntityTypeConfiguration<AlbumCountry>
{
    public void Configure(EntityTypeBuilder<AlbumCountry> entity)
    {
        entity.ToTable("album_countries");

        entity.HasKey(e => new { e.AlbumId, e.CountryId });

        entity.Property(e => e.AlbumId)
            .HasColumnName("album_id")
            .HasConversion(albumId => albumId.Value, dbId => AlbumId.Of(dbId));

        entity.Property(e => e.CountryId)
            .HasColumnName("country_id")
            .HasConversion(countryId => countryId.Value, dbId => CountryId.Of(dbId));

        entity.HasOne<Album>()
            .WithMany(a => a.AlbumCountries)
            .HasForeignKey(e => e.AlbumId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne<Country>()
            .WithMany()
            .HasForeignKey(e => e.CountryId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasIndex(e => e.CountryId);
    }
}
