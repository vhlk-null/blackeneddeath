using Library.Domain.Models;
using Library.Domain.Models.JoinTables;
using Library.Domain.ValueObjects.Ids;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Data.Configurations.JoinTables;

public class BandGenreConfiguration : IEntityTypeConfiguration<BandGenre>
{
    public void Configure(EntityTypeBuilder<BandGenre> entity)
    {
        entity.ToTable("band_genres");

        entity.HasKey(e => new { e.BandId, e.GenreId });

        entity.Property(e => e.BandId)
            .HasColumnName("band_id")
            .HasConversion(bandId => bandId.Value, dbId => BandId.Of(dbId));

        entity.Property(e => e.GenreId)
            .HasColumnName("genre_id")
            .HasConversion(genreId => genreId.Value, dbId => GenreId.Of(dbId));

        entity.Property(e => e.IsPrimary)
            .HasColumnName("is_primary");

        entity.HasOne<Band>()
            .WithMany(b => b.BandGenres)
            .HasForeignKey(e => e.BandId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne<Genre>()
            .WithMany()
            .HasForeignKey(e => e.GenreId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
