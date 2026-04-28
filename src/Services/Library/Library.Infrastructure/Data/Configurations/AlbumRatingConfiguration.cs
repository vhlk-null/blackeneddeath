using Library.Domain.Models;
using Library.Domain.ValueObjects.Ids;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Data.Configurations;

public class AlbumRatingConfiguration : IEntityTypeConfiguration<AlbumRating>
{
    public void Configure(EntityTypeBuilder<AlbumRating> entity)
    {
        entity.ToTable("album_ratings");

        entity.HasKey(e => new { e.UserId, e.AlbumId });

        entity.Property(e => e.UserId)
            .HasColumnName("user_id");

        entity.Property(e => e.AlbumId)
            .HasConversion(id => id.Value, v => AlbumId.Of(v))
            .HasColumnName("album_id");

        entity.Property(e => e.Rating)
            .HasColumnName("rating");

        entity.Property(e => e.RatedAt)
            .HasColumnName("rated_at");

        entity.HasOne(e => e.Album)
            .WithMany(a => a.AlbumRatings)
            .HasForeignKey(e => e.AlbumId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
