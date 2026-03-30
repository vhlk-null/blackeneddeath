using Library.Domain.Models;
using Library.Domain.ValueObjects.Ids;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Data.Configurations;

public class GenreCardConfiguration : IEntityTypeConfiguration<GenreCard>
{
    public void Configure(EntityTypeBuilder<GenreCard> entity)
    {
        entity.ToTable("genre_cards");

        entity.HasKey(g => g.Id);

        entity.Property(g => g.Id)
            .HasConversion(id => id.Value, dbId => GenreCardId.Of(dbId));

        entity.Property(g => g.Name)
            .HasMaxLength(200)
            .HasColumnName("name")
            .IsRequired();

        entity.HasIndex(g => g.Name).IsUnique();

        entity.Property(g => g.Description)
            .HasMaxLength(1000)
            .HasColumnName("description")
            .IsRequired();

        entity.Property(g => g.CoverUrl)
            .HasMaxLength(500)
            .HasColumnName("cover_url");

        entity.Property(g => g.OrderNumber)
            .HasColumnName("order_number")
            .IsRequired();
    }
}
