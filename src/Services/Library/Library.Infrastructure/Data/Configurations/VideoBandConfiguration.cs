using Library.Domain.Models;
using Library.Domain.ValueObjects.Ids;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Data.Configurations;

public class VideoBandConfiguration : IEntityTypeConfiguration<VideoBand>
{
    public void Configure(EntityTypeBuilder<VideoBand> entity)
    {
        entity.ToTable("video_bands");

        entity.HasKey(e => e.Id);

        entity.Property(e => e.Id)
            .HasConversion(id => id.Value, dbId => VideoBandId.Of(dbId));

        entity.Property(e => e.BandId)
            .HasColumnName("band_id")
            .HasConversion(id => id.Value, dbId => BandId.Of(dbId));

        entity.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(300)
            .HasColumnName("name");

        entity.Property(e => e.Year)
            .HasColumnName("year");

        entity.Property(e => e.CountryId)
            .HasColumnName("country_id")
            .HasConversion(
                id => id != null ? id.Value : (Guid?)null,
                dbId => dbId.HasValue ? CountryId.Of(dbId.Value) : null);

        entity.Property(e => e.VideoType)
            .HasConversion<string>()
            .HasColumnName("video_type");

        entity.Property(e => e.YoutubeLink)
            .IsRequired()
            .HasMaxLength(500)
            .HasColumnName("youtube_link");

        entity.Property(e => e.Info)
            .HasColumnType("text")
            .HasColumnName("info");

        entity.Property(e => e.IsApproved)
            .HasDefaultValue(false)
            .HasColumnName("is_approved");

        entity.HasOne<Band>()
            .WithMany(b => b.VideoBands)
            .HasForeignKey(e => e.BandId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne<Country>()
            .WithMany()
            .HasForeignKey(e => e.CountryId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
