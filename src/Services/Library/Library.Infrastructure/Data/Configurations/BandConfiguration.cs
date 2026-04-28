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

        entity.Property(e => e.Slug)
            .IsRequired()
            .HasMaxLength(220)
            .HasColumnName("slug");

        entity.HasIndex(e => e.Slug)
            .IsUnique()
            .HasDatabaseName("ix_bands_slug");

        entity.Property(e => e.Bio)
            .HasColumnType("text")
            .HasColumnName("bio");

        entity.Property(e => e.Status)
            .HasConversion<string>()
            .HasColumnName("status");

        entity.Property(e => e.LogoUrl)
            .HasMaxLength(500)
            .HasColumnName("logo_url");

        entity.Property(e => e.Facebook).HasMaxLength(500).HasColumnName("facebook");
        entity.Property(e => e.Youtube).HasMaxLength(500).HasColumnName("youtube");
        entity.Property(e => e.Instagram).HasMaxLength(500).HasColumnName("instagram");
        entity.Property(e => e.Twitter).HasMaxLength(500).HasColumnName("twitter");
        entity.Property(e => e.Website).HasMaxLength(500).HasColumnName("website");

        entity.Property(e => e.IsApproved)
            .HasDefaultValue(false)
            .HasColumnName("is_approved");

        entity.Property(e => e.AverageRating)
            .HasColumnName("average_rating");

        entity.Property(e => e.RatingsCount)
            .HasDefaultValue(0)
            .HasColumnName("ratings_count");

        entity.ComplexProperty(e => e.Activity, ba =>
        {
            ba.Property(a => a.FormedYear).HasColumnName("formed_year");
            ba.Property(a => a.DisbandedYear).HasColumnName("disbanded_year");
        });

        entity.Navigation(e => e.BandCountries).UsePropertyAccessMode(PropertyAccessMode.Field);
        entity.Navigation(e => e.BandGenres).UsePropertyAccessMode(PropertyAccessMode.Field);
        entity.Navigation(e => e.VideoBands).UsePropertyAccessMode(PropertyAccessMode.Field);
        entity.Navigation(e => e.BandRatings).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
