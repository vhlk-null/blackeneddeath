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

            modelBuilder.Entity<UserProfileInfo>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.Ignore(e => e.FavoriteGenres);
            });

            modelBuilder.Entity<FavoriteAlbum>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.UserId, e.AlbumId }).IsUnique();
            });

            modelBuilder.Entity<FavoriteBand>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.UserId, e.BandId }).IsUnique();
            });
        }
    }
}
