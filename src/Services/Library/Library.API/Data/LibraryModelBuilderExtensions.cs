using Library.API.Models;
using Library.API.Models.JoinTables;

namespace Library.API.Data
{
    public static class LibraryModelBuilderExtensions
    {
        public static void SetupAlbum(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Album>(entity =>
            {
                entity.ToTable("albums");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("title");

                entity.Property(e => e.ReleaseDate)
                    .HasColumnName("release_date");

                entity.Property(e => e.CoverUrl)
                    .HasMaxLength(500)
                    .HasColumnName("cover_url");

                entity.Property(e => e.Type)
                    .HasColumnName("type");

                entity.Property(e => e.Format)
                    .HasColumnName("format");

                entity.Property(e => e.Label)
                    .HasMaxLength(200)
                    .HasColumnName("label");
            });
        }

        public static void SetupBand(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Band>(entity =>
            {
                entity.ToTable("bands");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("name");

                entity.Property(e => e.Bio)
                    .HasColumnType("text")
                    .HasColumnName("bio");

                entity.Property(e => e.CountryId)
                    .HasColumnName("country_id");

                entity.Property(e => e.FormedYear)
                    .HasColumnName("formed_year");

                entity.Property(e => e.DisbandedYear)
                    .HasColumnName("disbanded_year");

                entity.Property(e => e.Status)
                    .HasColumnName("status");

                entity.Property(e => e.LogoUrl)
                    .HasMaxLength(500)
                    .HasColumnName("logo_url");

                entity.HasOne(e => e.Country)
                   .WithMany(c => c.Bands)
                   .HasForeignKey(e => e.CountryId)
                   .OnDelete(DeleteBehavior.SetNull);
            });
        }

        public static void SetupTrack(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Track>(entity =>
            {
                entity.ToTable("tracks");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("title");
            });
        }

        public static void SetupGenre(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Genre>(entity =>
            {
                entity.ToTable("genres");
                entity.HasKey(g => g.Id);

                entity.Property(g => g.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(g => g.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name")
                    .IsRequired();

                entity.Property(g => g.ParentGenreId)
                    .HasColumnName("parent_genre_id");

                entity.HasOne(g => g.ParentGenre)
                    .WithMany(g => g.SubGenres)
                    .HasForeignKey(g => g.ParentGenreId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(g => g.Name).IsUnique();
                entity.HasIndex(g => g.ParentGenreId);
            });
        }

        public static void SetupCountry(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>(entity =>
            {
                entity.ToTable("countries");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(2)
                    .HasColumnName("code");

                entity.HasIndex(e => e.Code).IsUnique();
            });
        }

        public static void SetupStreamingLink(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StreamingLink>(entity =>
            {
                entity.ToTable("streaming_links");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.AlbumId)
                    .HasColumnName("album_id");

                entity.Property(e => e.Platform)
                    .HasColumnName("platform");

                entity.Property(e => e.EmbedCode)
                    .HasColumnType("text")
                    .HasColumnName("embed_code");

                entity.HasOne(e => e.Album)
                    .WithMany(a => a.StreamingLinks)
                    .HasForeignKey(e => e.AlbumId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        public static void SetupAlbumBand(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AlbumBand>(entity =>
            {
                entity.ToTable("album_bands");

                entity.HasKey(e => new { e.AlbumId, e.BandId });

                entity.Property(e => e.AlbumId)
                    .HasColumnName("album_id");

                entity.Property(e => e.BandId)
                    .HasColumnName("band_id");

                entity.HasOne(e => e.Album)
                    .WithMany(a => a.Bands)
                    .HasForeignKey(e => e.AlbumId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Band)
                    .WithMany(b => b.Albums)
                    .HasForeignKey(e => e.BandId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        public static void SetupAlbumGenre(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AlbumGenre>(entity =>
            {
                entity.ToTable("album_genres");

                entity.HasKey(e => new { e.AlbumId, e.GenreId });

                entity.Property(e => e.AlbumId)
                    .HasColumnName("album_id");

                entity.Property(e => e.GenreId)
                    .HasColumnName("genre_id");

                entity.HasOne(e => e.Album)
                    .WithMany(a => a.Genres)
                    .HasForeignKey(e => e.AlbumId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Genre)
                    .WithMany(g => g.Albums)
                    .HasForeignKey(e => e.GenreId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        public static void SetupAlbumTrack(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AlbumTrack>(entity =>
            {
                entity.ToTable("album_tracks");

                entity.HasKey(e => new { e.AlbumId, e.TrackId });

                entity.Property(e => e.AlbumId)
                    .HasColumnName("album_id");

                entity.Property(e => e.TrackId)
                    .HasColumnName("track_id");

                entity.HasOne(e => e.Album)
                    .WithMany(a => a.Tracks)
                    .HasForeignKey(e => e.AlbumId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Track)
                    .WithMany(t => t.Albums)
                    .HasForeignKey(e => e.TrackId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        public static void SetupBandGenre(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BandGenre>(entity =>
            {
                entity.ToTable("band_genres");

                entity.HasKey(e => new { e.BandId, e.GenreId });

                entity.Property(e => e.BandId)
                    .HasColumnName("band_id");

                entity.Property(e => e.GenreId)
                    .HasColumnName("genre_id");

                entity.HasOne(e => e.Band)
                    .WithMany(b => b.Genres)
                    .HasForeignKey(e => e.BandId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Genre)
                    .WithMany(g => g.Bands)
                    .HasForeignKey(e => e.GenreId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        public static void SetupAlbumCountry(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AlbumCountry>(entity =>
            {
                entity.ToTable("album_countries");

                entity.HasKey(e => new { e.AlbumId, e.CountryId });

                entity.Property(e => e.AlbumId)
                    .HasColumnName("album_id");

                entity.Property(e => e.CountryId)
                    .HasColumnName("country_id");

                entity.HasOne(e => e.Album)
                    .WithMany(a => a.Countries)
                    .HasForeignKey(e => e.AlbumId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Country)
                    .WithMany(c => c.Albums)
                    .HasForeignKey(e => e.CountryId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => e.CountryId);
            });
        }
    }
}