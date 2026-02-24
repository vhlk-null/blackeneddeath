using Library.Domain.Models;
using Library.Domain.Models.JoinTables;
using Library.Domain.ValueObjects.Ids;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Data.Configurations;

public class AlbumTrackConfiguration : IEntityTypeConfiguration<AlbumTrack>
{
    public void Configure(EntityTypeBuilder<AlbumTrack> entity)
    {
        entity.ToTable("album_tracks");

        entity.HasKey(e => new { e.AlbumId, e.TrackId });

        entity.Property(e => e.AlbumId)
            .HasColumnName("album_id")
            .HasConversion(albumId => albumId.Value, dbId => AlbumId.Of(dbId));

        entity.Property(e => e.TrackId)
            .HasColumnName("track_id")
            .HasConversion(trackId => trackId.Value, dbId => TrackId.Of(dbId));

        entity.Property(e => e.TrackNumber)
            .HasColumnName("track_number");

        entity.HasOne<Album>()
            .WithMany(a => a.AlbumTracks)
            .HasForeignKey(e => e.AlbumId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne<Track>()
            .WithMany()
            .HasForeignKey(e => e.TrackId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
