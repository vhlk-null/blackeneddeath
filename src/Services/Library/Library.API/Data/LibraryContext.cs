using Library.API.Models;
using Library.API.Models.JoinTables;

namespace Library.API.Data
{
    public class LibraryContext : DbContext
    {
        public LibraryContext()
        {
        }

        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
        {
        }

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
        public virtual DbSet<AlbumCountry> AlbumCountries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.SetupGenre();
            modelBuilder.SetupCountry();
            modelBuilder.SetupTrack();
            modelBuilder.SetupBand();
            modelBuilder.SetupAlbum();
            modelBuilder.SetupStreamingLink();
            modelBuilder.SetupAlbumBand();
            modelBuilder.SetupAlbumGenre();
            modelBuilder.SetupAlbumCountry();
            modelBuilder.SetupAlbumTrack();
            modelBuilder.SetupBandGenre();
        }
    }
}