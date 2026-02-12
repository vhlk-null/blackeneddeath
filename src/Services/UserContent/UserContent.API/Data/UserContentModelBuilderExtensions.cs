namespace UserContent.API.Data
{
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
                    .IsRequired()
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

                entity.Property(e => e.CoverUrl)
                    .HasMaxLength(500)
                    .HasColumnName("cover_url");

                entity.Property(e => e.ReleaseDate)
                    .HasColumnName("release_date");
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

                entity.Property(e => e.LogoUrl)
                    .HasMaxLength(500)
                    .HasColumnName("logo_url");

                entity.Property(e => e.ReleaseDate)
                    .HasColumnName("release_date");
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

                entity.Property(e => e.UserRating)
                    .HasColumnName("user_rating");

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
                    .OnDelete(DeleteBehavior.Restrict);
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
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
