using Library.Domain.Models;
using Library.Domain.Models.JoinTables;
using Library.Domain.ValueObjects.Ids;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Data.Configurations.JoinTables;

public class AlbumGenreConfiguration : IEntityTypeConfiguration<AlbumGenre>
{
    public void Configure(EntityTypeBuilder<AlbumGenre> entity)
    {
        entity.ToTable("album_genres");

        entity.HasKey(e => new { e.AlbumId, e.GenreId });

        entity.Property(e => e.AlbumId)
            .HasColumnName("album_id")
            .HasConversion(albumId => albumId.Value, dbId => AlbumId.Of(dbId));

        entity.Property(e => e.GenreId)
            .HasColumnName("genre_id")
            .HasConversion(genreId => genreId.Value, dbId => GenreId.Of(dbId));

        entity.Property(e => e.IsPrimary)
            .HasColumnName("is_primary");

        entity.HasOne<Album>()
            .WithMany(a => a.AlbumGenres)
            .HasForeignKey(e => e.AlbumId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne<Genre>()
            .WithMany()
            .HasForeignKey(e => e.GenreId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
