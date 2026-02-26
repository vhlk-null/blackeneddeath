using System.Reflection;
using Library.Domain.Models;
using Library.Domain.Models.JoinTables;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Data;

public class LibraryContext : DbContext
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
    public virtual DbSet<StreamingLink> StreamingLinks { get; set; }

    // Junction tables (Many-to-Many)
    public virtual DbSet<AlbumBand> AlbumBands { get; set; }
    public virtual DbSet<AlbumGenre> AlbumGenres { get; set; }
    public virtual DbSet<AlbumTrack> AlbumTracks { get; set; }
    public virtual DbSet<BandGenre> BandGenres { get; set; }
    public virtual DbSet<BandCountry> BandCountries { get; set; }
    public virtual DbSet<AlbumCountry> AlbumCountries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
}