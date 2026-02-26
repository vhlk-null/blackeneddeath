using Library.Domain.Models;
using Library.Domain.ValueObjects.Ids;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Data.Configurations;

public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> entity)
    {
        entity.ToTable("countries");

        entity.HasKey(e => e.Id);

        entity.Property(e => e.Id)
            .HasColumnName("id")
            .HasConversion(countryId => countryId.Value, dbId => CountryId.Of(dbId))
            .ValueGeneratedNever();

        entity.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("name");

        entity.Property(e => e.Code)
            .IsRequired()
            .HasMaxLength(2)
            .HasColumnName("code");

        entity.HasIndex(e => e.Code).IsUnique();
    }
}
