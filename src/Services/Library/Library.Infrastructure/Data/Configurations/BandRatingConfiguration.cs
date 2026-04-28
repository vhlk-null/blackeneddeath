using Library.Domain.Models;
using Library.Domain.ValueObjects.Ids;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Data.Configurations;

public class BandRatingConfiguration : IEntityTypeConfiguration<BandRating>
{
    public void Configure(EntityTypeBuilder<BandRating> entity)
    {
        entity.ToTable("band_ratings");

        entity.HasKey(e => new { e.UserId, e.BandId });

        entity.Property(e => e.UserId)
            .HasColumnName("user_id");

        entity.Property(e => e.BandId)
            .HasConversion(id => id.Value, v => BandId.Of(v))
            .HasColumnName("band_id");

        entity.Property(e => e.Rating)
            .HasColumnName("rating");

        entity.Property(e => e.RatedAt)
            .HasColumnName("rated_at");

        entity.HasOne(e => e.Band)
            .WithMany(b => b.BandRatings)
            .HasForeignKey(e => e.BandId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
