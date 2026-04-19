namespace UserContent.Infrastructure.Data;

public static class UserContentModelBuilderExtensions
{
    public static void SetupUserProfileInfo(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserProfileInfo>(entity =>
        {
            entity.ToTable("user_profiles");

            entity.HasKey(e => e.UserId);

            entity.Property(e => e.UserId)
                .HasColumnName("user_id")
                .ValueGeneratedNever();

            entity.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("username");

            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");

            entity.Property(e => e.AvatarUrl)
                .HasMaxLength(500)
                .HasColumnName("avatar_url");

            entity.Property(e => e.RegisteredDate)
                .HasColumnName("registered_date");

            entity.Property(e => e.LastLoginDate)
                .HasColumnName("last_login_date");

            entity.Property(e => e.Bio)
                .HasColumnType("text")
                .HasColumnName("bio");

            entity.Ignore(e => e.FavoriteBandsCount);
            entity.Ignore(e => e.FavoriteAlbumsCount);
            entity.Ignore(e => e.ReviewsCount);

            entity.HasIndex(e => e.Username).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();
        });
    }

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

            entity.Property(e => e.Slug)
                .HasMaxLength(300)
                .HasColumnName("slug");

            entity.Property(e => e.CoverUrl)
                .HasMaxLength(500)
                .HasColumnName("cover_url");

            entity.Property(e => e.ReleaseDate)
                .HasColumnName("release_date");

            entity.Property(e => e.Format)
                .HasColumnName("format");

            entity.Property(e => e.Type)
                .HasColumnName("type");

            entity.Property(e => e.PrimaryGenreName)
                .HasMaxLength(100)
                .HasColumnName("primary_genre_name");

            entity.Property(e => e.PrimaryGenreSlug)
                .HasMaxLength(150)
                .HasColumnName("primary_genre_slug");

            entity.Property(e => e.BandIds)
                .HasColumnType("text")
                .HasColumnName("band_ids");

            entity.Property(e => e.BandNames)
                .HasColumnType("text")
                .HasColumnName("band_names");

            entity.Property(e => e.BandSlugs)
                .HasColumnType("text")
                .HasColumnName("band_slugs");

            entity.Property(e => e.CountryNames)
                .HasColumnType("text")
                .HasColumnName("country_names");

            entity.Property(e => e.CountryCodes)
                .HasColumnType("text")
                .HasColumnName("country_codes");

            entity.Property(e => e.AverageRating)
                .HasColumnName("average_rating");

            entity.Property(e => e.RatingsCount)
                .HasColumnName("ratings_count");
        });
    }

    public static void SetupBand(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Band>(entity =>
        {
            entity.ToTable("bands");

            entity.HasKey(e => e.BandId);

            entity.Property(e => e.BandId)
                .HasColumnName("band_id")
                .ValueGeneratedNever();

            entity.Property(e => e.BandName)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("band_name");

            entity.Property(e => e.Slug)
                .HasMaxLength(300)
                .HasColumnName("slug");

            entity.Property(e => e.LogoUrl)
                .HasMaxLength(500)
                .HasColumnName("logo_url");

            entity.Property(e => e.FormedYear)
                .HasColumnName("formed_year");

            entity.Property(e => e.DisbandedYear)
                .HasColumnName("disbanded_year");

            entity.Property(e => e.Status)
                .HasColumnName("status");

            entity.Property(e => e.PrimaryGenreName)
                .HasMaxLength(100)
                .HasColumnName("primary_genre_name");

            entity.Property(e => e.PrimaryGenreSlug)
                .HasMaxLength(150)
                .HasColumnName("primary_genre_slug");

            entity.Property(e => e.CountryNames)
                .HasColumnType("text")
                .HasColumnName("country_names");

            entity.Property(e => e.CountryCodes)
                .HasColumnType("text")
                .HasColumnName("country_codes");

            entity.Property(e => e.AverageRating)
                .HasColumnName("average_rating");

            entity.Property(e => e.RatingsCount)
                .HasColumnName("ratings_count");
        });
    }

    public static void SetupFavoriteAlbum(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FavoriteAlbum>(entity =>
        {
            entity.ToTable("favorite_albums");

            entity.HasKey(e => new { e.UserId, e.AlbumId });

            entity.Property(e => e.UserId)
                .HasColumnName("user_id");

            entity.Property(e => e.AlbumId)
                .HasColumnName("album_id");

            entity.Property(e => e.AddedDate)
                .HasColumnName("added_date");

            entity.Property(e => e.UserReview)
                .HasColumnType("text")
                .HasColumnName("user_review");

            entity.HasOne(e => e.User)
                .WithMany(u => u.FavoriteAlbums)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Album)
                .WithMany(a => a.FavoriteAlbums)
                .HasForeignKey(e => e.AlbumId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    public static void SetupFavoriteBand(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FavoriteBand>(entity =>
        {
            entity.ToTable("favorite_bands");

            entity.HasKey(e => new { e.UserId, e.BandId });

            entity.Property(e => e.UserId)
                .HasColumnName("user_id");

            entity.Property(e => e.BandId)
                .HasColumnName("band_id");

            entity.Property(e => e.AddedDate)
                .HasColumnName("added_date");

            entity.Property(e => e.IsFollowing)
                .HasColumnName("is_following");

            entity.HasOne(e => e.User)
                .WithMany(u => u.FavoriteBands)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Band)
                .WithMany(b => b.FavoriteBands)
                .HasForeignKey(e => e.BandId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    public static void SetupCollectionBand(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CollectionBand>(entity =>
        {
            entity.ToTable("collection_bands");

            entity.HasKey(e => new { e.CollectionId, e.BandId });

            entity.Property(e => e.CollectionId)
                .HasColumnName("collection_id");

            entity.Property(e => e.BandId)
                .HasColumnName("band_id");

            entity.Property(e => e.AddedDate)
                .HasColumnName("added_date");

            entity.HasOne(e => e.Collection)
                .WithMany(c => c.CollectionBands)
                .HasForeignKey(e => e.CollectionId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Band)
                .WithMany(b => b.CollectionBands)
                .HasForeignKey(e => e.BandId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    public static void SetupAlbumReview(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AlbumReview>(entity =>
        {
            entity.ToTable("album_reviews");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedNever();

            entity.Property(e => e.AlbumId)
                .HasColumnName("album_id");

            entity.Property(e => e.UserId)
                .HasColumnName("user_id");

            entity.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("username");

            entity.Property(e => e.Title)
                .HasMaxLength(120)
                .HasColumnName("title");

            entity.Property(e => e.Body)
                .HasColumnType("text")
                .HasColumnName("body");

            entity.Property(e => e.Rating)
                .HasColumnName("rating");

            entity.Property(e => e.RatedAt)
                .HasColumnName("rated_at");

            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at");

            entity.HasOne(e => e.Album)
                .WithMany(a => a.Reviews)
                .HasForeignKey(e => e.AlbumId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    public static void SetupCollection(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Collection>(entity =>
        {
            entity.ToTable("collections");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedNever();

            entity.Property(e => e.UserId)
                .HasColumnName("user_id");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("name");

            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");

            entity.Property(e => e.Type)
                .HasColumnName("type");

            entity.Property(e => e.CoverUrl)
                .HasMaxLength(500)
                .HasColumnName("cover_url");

            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at");

            entity.HasOne(e => e.User)
                .WithMany(u => u.Collections)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    public static void SetupCollectionAlbum(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CollectionAlbum>(entity =>
        {
            entity.ToTable("collection_albums");

            entity.HasKey(e => new { e.CollectionId, e.AlbumId });

            entity.Property(e => e.CollectionId)
                .HasColumnName("collection_id");

            entity.Property(e => e.AlbumId)
                .HasColumnName("album_id");

            entity.Property(e => e.AddedDate)
                .HasColumnName("added_date");

            entity.HasOne(e => e.Collection)
                .WithMany(c => c.CollectionAlbums)
                .HasForeignKey(e => e.CollectionId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Album)
                .WithMany(a => a.CollectionAlbums)
                .HasForeignKey(e => e.AlbumId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

public static void SetupBandReview(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BandReview>(entity =>
        {
            entity.ToTable("band_reviews");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedNever();

            entity.Property(e => e.BandId)
                .HasColumnName("band_id");

            entity.Property(e => e.UserId)
                .HasColumnName("user_id");

            entity.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("username");

            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(120)
                .HasColumnName("title");

            entity.Property(e => e.Body)
                .HasColumnType("text")
                .HasColumnName("body");

            entity.Property(e => e.Rating)
                .HasColumnName("rating");

            entity.Property(e => e.RatedAt)
                .HasColumnName("rated_at");

            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at");

            entity.HasOne(e => e.Band)
                .WithMany(b => b.Reviews)
                .HasForeignKey(e => e.BandId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
