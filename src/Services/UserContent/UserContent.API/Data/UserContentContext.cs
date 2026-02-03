using Microsoft.EntityFrameworkCore;

namespace UserContent.API.Data
{
    public class UserContentContext : DbContext
    {
        public UserContentContext()
        {
        }

        public UserContentContext(DbContextOptions<UserContentContext> options) : base(options)
        {
        }

        public virtual DbSet<UserProfileInfo> UserProfiles { get; set; }
        public virtual DbSet<FavoriteAlbum> FavoriteAlbums { get; set; }
        public virtual DbSet<FavoriteBand> FavoriteBands { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.SetupUserProfileInfo();
            modelBuilder.SetupFavoriteAlbum();
            modelBuilder.SetupFavoriteBand();
        }
    }
}
