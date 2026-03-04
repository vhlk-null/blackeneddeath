namespace UserContent.Infrastructure.Repositories;

public class UserContentRepository(UserContentContext context) : IUserContentRepository
{
    public async Task<UserProfileInfo?> GetUserProfileWithDetailsAsync(Guid userId, CancellationToken ct = default)
        => await context.UserProfiles
            .Include(u => u.FavoriteAlbums).ThenInclude(fa => fa.Album)
            .Include(u => u.FavoriteBands).ThenInclude(fb => fb.Band)
            .FirstOrDefaultAsync(u => u.UserId == userId, ct);

    public async Task<Album?> GetAlbumAsync(Guid albumId, CancellationToken ct = default)
        => await context.Albums.FirstOrDefaultAsync(a => a.Id == albumId, ct);

    public async Task<FavoriteAlbum?> GetFavoriteAlbumAsync(Guid userId, Guid albumId, CancellationToken ct = default)
        => await context.FavoriteAlbums
            .FirstOrDefaultAsync(fa => fa.UserId == userId && fa.AlbumId == albumId, ct);

    public async Task<FavoriteBand?> GetFavoriteBandAsync(Guid userId, Guid bandId, CancellationToken ct = default)
        => await context.FavoriteBands
            .FirstOrDefaultAsync(fb => fb.UserId == userId && fb.BandId == bandId, ct);

    public async Task AddAsync<T>(T entity, CancellationToken ct = default) where T : class
        => await context.Set<T>().AddAsync(entity, ct);

    public void Remove<T>(T entity) where T : class
        => context.Set<T>().Remove(entity);

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await context.SaveChangesAsync(ct);
}
