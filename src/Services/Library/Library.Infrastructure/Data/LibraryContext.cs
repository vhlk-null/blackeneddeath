using System.Reflection;
using Library.Application.Data;
using Library.Domain.Models;
using Library.Domain.Models.JoinTables;

namespace Library.Infrastructure.Data;

public class LibraryContext : DbContext, ILibraryDbContext
{
    public LibraryContext() { }

    public LibraryContext(DbContextOptions<LibraryContext> options)
        : base(options) { }

    // Main entities
    public virtual DbSet<Album> Albums { get; set; }
    public virtual DbSet<Band> Bands { get; set; }
    public virtual DbSet<Track> Tracks { get; set; }
    public virtual DbSet<Genre> Genres { get; set; }
    public virtual DbSet<Country> Countries { get; set; }
    public virtual DbSet<Label> Labels { get; set; }
    public virtual DbSet<Tag> Tags { get; set; }
    public virtual DbSet<StreamingLink> StreamingLinks { get; set; }
    public virtual DbSet<GenreCard> GenreCards { get; set; }
    public virtual DbSet<VideoBand> VideoBands { get; set; }

    // Junction tables (Many-to-Many)
    public virtual DbSet<AlbumBand> AlbumBands { get; set; }
    public virtual DbSet<AlbumGenre> AlbumGenres { get; set; }
    public virtual DbSet<AlbumTrack> AlbumTracks { get; set; }
    public virtual DbSet<AlbumTag> AlbumTags { get; set; }
    public virtual DbSet<BandGenre> BandGenres { get; set; }
    public virtual DbSet<BandCountry> BandCountries { get; set; }
    public virtual DbSet<AlbumCountry> AlbumCountries { get; set; }
    public virtual DbSet<GenreCardGenre> GenreCardGenres { get; set; }
    public virtual DbSet<GenreCardTag> GenreCardTags { get; set; }

    // Ratings
    public virtual DbSet<AlbumRating> AlbumRatings { get; set; }
    public virtual DbSet<BandRating> BandRatings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
}