namespace UserContent.Infrastructure.Data;

public class UserContentContext : DbContext
{
    public UserContentContext()
    {
    }

    public UserContentContext(DbContextOptions<UserContentContext> options) : base(options)
    {
    }

    public virtual DbSet<UserProfileInfo> UserProfiles { get; set; }
    public virtual DbSet<Album> Albums { get; set; }
    public virtual DbSet<Band> Bands { get; set; }
    public virtual DbSet<FavoriteAlbum> FavoriteAlbums { get; set; }
    public virtual DbSet<FavoriteBand> FavoriteBands { get; set; }
    public virtual DbSet<AlbumReview> AlbumReviews { get; set; }
    public virtual DbSet<BandReview> BandReviews { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.SetupUserProfileInfo();
        modelBuilder.SetupAlbum();
        modelBuilder.SetupBand();
        modelBuilder.SetupFavoriteAlbum();
        modelBuilder.SetupFavoriteBand();
        modelBuilder.SetupAlbumReview();
        modelBuilder.SetupBandReview();
    }
}
