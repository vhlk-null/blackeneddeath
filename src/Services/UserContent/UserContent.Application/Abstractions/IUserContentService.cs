namespace UserContent.Application.Abstractions;

public interface IUserContentService
{
    Task<UserProfileDto> GetUserProfileAsync(Guid userId, CancellationToken ct = default);
    Task<Guid> AddFavoriteAlbumAsync(Guid userId, Guid albumId, CancellationToken ct = default);
    Task DeleteFavoriteAlbumAsync(Guid userId, Guid albumId, CancellationToken ct = default);
    Task<Guid> AddFavoriteBandAsync(Guid userId, Guid bandId, CancellationToken ct = default);
    Task DeleteFavoriteBandAsync(Guid userId, Guid bandId, CancellationToken ct = default);
    Task<double?> RateAlbumAsync(Guid userId, Guid albumId, int rating, CancellationToken ct = default);
    Task RateBandAsync(Guid userId, Guid bandId, int rating, CancellationToken ct = default);
    Task<(int? UserRating, double? AverageRating)> GetAlbumRatingAsync(Guid userId, Guid albumId, CancellationToken ct = default);
    Task<double?> GetAlbumAverageRatingAsync(Guid albumId, CancellationToken ct = default);
    Task<int?> GetBandRatingAsync(Guid userId, Guid bandId, CancellationToken ct = default);
}
