namespace Library.Application.Data;

public interface ILibraryDbContext
{
    DbSet<Album> Albums { get; }
    DbSet<Band> Bands { get; }
    DbSet<Track> Tracks { get; }
    DbSet<Genre> Genres { get; }
    DbSet<Country> Countries { get; }
    DbSet<Label> Labels { get; }
    DbSet<Tag> Tags { get; }
    DbSet<StreamingLink> StreamingLinks { get; }
    DbSet<GenreCard> GenreCards { get; }
    DbSet<VideoBand> VideoBands { get; }

    DbSet<AlbumBand> AlbumBands { get; }
    DbSet<AlbumGenre> AlbumGenres { get; }
    DbSet<AlbumTrack> AlbumTracks { get; }
    DbSet<AlbumTag> AlbumTags { get; }
    DbSet<BandGenre> BandGenres { get; }
    DbSet<BandCountry> BandCountries { get; }
    DbSet<AlbumCountry> AlbumCountries { get; }
    DbSet<GenreCardGenre> GenreCardGenres { get; }
    DbSet<GenreCardTag> GenreCardTags { get; }

    DbSet<AlbumRating> AlbumRatings { get; }
    DbSet<BandRating> BandRatings { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    Task<List<Genre>> GetAllGenresAsync(CancellationToken cancellationToken = default);
    Task<List<Country>> GetAllCountriesAsync(CancellationToken cancellationToken = default);
    Task<List<Label>> GetAllLabelsAsync(CancellationToken cancellationToken = default);
    Task<List<Tag>> GetAllTagsAsync(CancellationToken cancellationToken = default);
    Task<List<GenreCard>> GetAllGenreCardsAsync(CancellationToken cancellationToken = default);
}