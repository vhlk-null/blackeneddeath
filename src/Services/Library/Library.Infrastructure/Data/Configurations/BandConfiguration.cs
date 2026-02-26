using Library.Domain.Models;
using Library.Domain.ValueObjects.Ids;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Data.Configurations;

public class BandConfiguration : IEntityTypeConfiguration<Band>
{
    public void Configure(EntityTypeBuilder<Band> entity)
    {
        entity.ToTable("bands");

        entity.HasKey(e => e.Id);

        entity.Property(e => e.Id)
            .HasConversion(bandId => bandId.Value, dbId => BandId.Of(dbId));

        entity.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("name");

        entity.Property(e => e.Bio)
            .HasColumnType("text")
            .HasColumnName("bio");

        entity.Property(e => e.Status)
            .HasConversion<string>()
            .HasColumnName("status");

        entity.Property(e => e.LogoUrl)
            .HasMaxLength(500)
            .HasColumnName("logo_url");

        entity.ComplexProperty(e => e.Activity, ba =>
        {
            ba.Property(a => a.FormedYear).HasColumnName("formed_year");
            ba.Property(a => a.DisbandedYear).HasColumnName("disbanded_year");
        });

        entity.Navigation(e => e.BandCountries).UsePropertyAccessMode(PropertyAccessMode.Field);
        entity.Navigation(e => e.BandGenres).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
