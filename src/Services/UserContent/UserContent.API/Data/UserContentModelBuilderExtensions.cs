using Microsoft.EntityFrameworkCore;

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

                entity.HasMany(e => e.FavoriteBands)
                    .WithOne()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.FavoriteAlbums)
                    .WithOne()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
            });
        }

        public static void SetupFavoriteAlbum(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FavoriteAlbum>(entity =>
            {
                entity.ToTable("favorite_albums");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id");

                entity.Property(e => e.AlbumId)
                    .HasColumnName("album_id");

                entity.Property(e => e.AlbumTitle)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("album_title");

                entity.Property(e => e.CoverUrl)
                    .HasMaxLength(500)
                    .HasColumnName("cover_url");

                entity.Property(e => e.ReleaseYear)
                    .HasColumnName("release_year");

                entity.Property(e => e.AddedDate)
                    .HasColumnName("added_date");

                entity.Property(e => e.UserRating)
                    .HasColumnName("user_rating");

                entity.Property(e => e.UserReview)
                    .HasColumnType("text")
                    .HasColumnName("user_review");

                entity.HasIndex(e => new { e.UserId, e.AlbumId }).IsUnique();
                entity.HasIndex(e => e.UserId);
            });
        }

        public static void SetupFavoriteBand(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FavoriteBand>(entity =>
            {
                entity.ToTable("favorite_bands");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id");

                entity.Property(e => e.BandId)
                    .HasColumnName("band_id");

                entity.Property(e => e.BandName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("band_name");

                entity.Property(e => e.LogoUrl)
                    .HasMaxLength(500)
                    .HasColumnName("logo_url");

                entity.Property(e => e.FormedYear)
                    .HasColumnName("formed_year");

                entity.Property(e => e.AddedDate)
                    .HasColumnName("added_date");

                entity.Property(e => e.IsFollowing)
                    .HasColumnName("is_following");

                entity.HasIndex(e => new { e.UserId, e.BandId }).IsUnique();
                entity.HasIndex(e => e.UserId);
            });
        }
    }
}
