using Library.Domain.Models;
using Library.Domain.Models.JoinTables;
using Library.Domain.ValueObjects.Ids;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Data.Configurations;

public class BandCountryConfiguration : IEntityTypeConfiguration<BandCountry>
{
    public void Configure(EntityTypeBuilder<BandCountry> entity)
    {
        entity.ToTable("band_countries");

        entity.HasKey(e => new { e.BandId, e.CountryId });

        entity.Property(e => e.BandId)
            .HasColumnName("band_id")
            .HasConversion(bandId => bandId.Value, dbId => BandId.Of(dbId));

        entity.Property(e => e.CountryId)
            .HasColumnName("country_id")
            .HasConversion(countryId => countryId.Value, dbId => CountryId.Of(dbId));

        entity.HasOne<Band>()
            .WithMany(b => b.BandCountries)
            .HasForeignKey(e => e.BandId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne<Country>()
            .WithMany()
            .HasForeignKey(e => e.CountryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
