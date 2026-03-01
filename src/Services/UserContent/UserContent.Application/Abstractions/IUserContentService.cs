namespace UserContent.Application.Abstractions;

public interface IUserContentService
{
    Task<UserProfileDto> GetUserProfileAsync(Guid userId, CancellationToken ct = default);
    Task<Guid> AddFavoriteAlbumAsync(Guid userId, Guid albumId, CancellationToken ct = default);
    Task DeleteFavoriteAlbumAsync(Guid userId, Guid albumId, CancellationToken ct = default);
    Task<Guid> AddFavoriteBandAsync(Guid userId, Guid bandId, CancellationToken ct = default);
    Task DeleteFavoriteBandAsync(Guid userId, Guid bandId, CancellationToken ct = default);
}
