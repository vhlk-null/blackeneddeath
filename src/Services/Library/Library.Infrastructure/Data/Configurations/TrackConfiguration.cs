using Library.Domain.Models;
using Library.Domain.ValueObjects.Ids;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Data.Configurations;

public class TrackConfiguration : IEntityTypeConfiguration<Track>
{
    public void Configure(EntityTypeBuilder<Track> entity)
    {
        entity.ToTable("tracks");

        entity.HasKey(e => e.Id);

        entity.Property(e => e.Id)
            .HasConversion(trackId => trackId.Value, dbId => TrackId.Of(dbId));

        entity.Property(e => e.Title)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("title");
    }
}
