namespace UserContent.Application.Abstractions;

public interface IUserContentRepository
{
    Task<UserProfileInfo?> GetUserProfileWithDetailsAsync(Guid userId, CancellationToken ct = default);
    Task<Album?> GetAlbumAsync(Guid albumId, CancellationToken ct = default);
    Task<FavoriteAlbum?> GetFavoriteAlbumAsync(Guid userId, Guid albumId, CancellationToken ct = default);
    Task<FavoriteBand?> GetFavoriteBandAsync(Guid userId, Guid bandId, CancellationToken ct = default);
    Task AddAsync<T>(T entity, CancellationToken ct = default) where T : class;
    void Remove<T>(T entity) where T : class;
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
