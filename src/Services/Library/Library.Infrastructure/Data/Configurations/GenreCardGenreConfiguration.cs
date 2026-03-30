using Library.Domain.Models;
using Library.Domain.Models.JoinTables;
using Library.Domain.ValueObjects.Ids;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Data.Configurations;

public class GenreCardGenreConfiguration : IEntityTypeConfiguration<GenreCardGenre>
{
    public void Configure(EntityTypeBuilder<GenreCardGenre> entity)
    {
        entity.ToTable("genre_card_genres");

        entity.HasKey(e => new { e.GenreCardId, e.GenreId });

        entity.Property(e => e.GenreCardId)
            .HasColumnName("genre_card_id")
            .HasConversion(id => id.Value, dbId => GenreCardId.Of(dbId));

        entity.Property(e => e.GenreId)
            .HasColumnName("genre_id")
            .HasConversion(id => id.Value, dbId => GenreId.Of(dbId));

        entity.HasOne<GenreCard>()
            .WithMany(g => g.GenreCardGenres)
            .HasForeignKey(e => e.GenreCardId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne<Genre>()
            .WithMany()
            .HasForeignKey(e => e.GenreId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
