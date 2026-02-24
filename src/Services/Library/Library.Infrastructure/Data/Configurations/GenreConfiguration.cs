using Library.Domain.Models;
using Library.Domain.ValueObjects.Ids;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Data.Configurations;

public class GenreConfiguration : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> entity)
    {
        entity.ToTable("genres");

        entity.HasKey(g => g.Id);

        entity.Property(g => g.Id)
            .HasConversion(genreId => genreId.Value, dbId => GenreId.Of(dbId));

        entity.Property(g => g.Name)
            .HasMaxLength(100)
            .HasColumnName("name")
            .IsRequired();

        entity.Property(g => g.ParentGenreId)
            .HasColumnName("parent_genre_id")
            .HasConversion(
                genreId => genreId != null ? genreId.Value : (Guid?)null,
                dbId => dbId.HasValue ? GenreId.Of(dbId.Value) : null);

        entity.Ignore(g => g.SubGenreIds);

        entity.HasOne<Genre>()
            .WithMany()
            .HasForeignKey(g => g.ParentGenreId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasIndex(g => g.Name).IsUnique();
        entity.HasIndex(g => g.ParentGenreId);
    }
}
