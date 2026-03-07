namespace Library.Application.Data;

public interface ILibraryDbContext
{
    DbSet<Album> Albums { get; }
    DbSet<Band> Bands { get; }
    DbSet<Track> Tracks { get; }
    DbSet<Genre> Genres { get; }
    DbSet<Country> Countries { get; }
    DbSet<StreamingLink> StreamingLinks { get; }

    DbSet<AlbumBand> AlbumBands { get; }
    DbSet<AlbumGenre> AlbumGenres { get; }
    DbSet<AlbumTrack> AlbumTracks { get; }
    DbSet<BandGenre> BandGenres { get; }
    DbSet<BandCountry> BandCountries { get; }
    DbSet<AlbumCountry> AlbumCountries { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}