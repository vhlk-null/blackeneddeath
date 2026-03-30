using Library.Domain.Models;
using Library.Domain.Models.JoinTables;
using Library.Domain.ValueObjects.Ids;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Data.Configurations;

public class GenreCardTagConfiguration : IEntityTypeConfiguration<GenreCardTag>
{
    public void Configure(EntityTypeBuilder<GenreCardTag> entity)
    {
        entity.ToTable("genre_card_tags");

        entity.HasKey(e => new { e.GenreCardId, e.TagId });

        entity.Property(e => e.GenreCardId)
            .HasColumnName("genre_card_id")
            .HasConversion(id => id.Value, dbId => GenreCardId.Of(dbId));

        entity.Property(e => e.TagId)
            .HasColumnName("tag_id")
            .HasConversion(id => id.Value, dbId => TagId.Of(dbId));

        entity.HasOne<GenreCard>()
            .WithMany(g => g.GenreCardTags)
            .HasForeignKey(e => e.GenreCardId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne<Tag>()
            .WithMany()
            .HasForeignKey(e => e.TagId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
