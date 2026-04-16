namespace UserContent.Application.Abstractions;

public interface IUserContentService
{
    Task<UserProfileDto> GetUserProfileAsync(Guid userId, CancellationToken ct = default);
    Task<Guid> AddFavoriteAlbumAsync(Guid userId, Guid albumId, CancellationToken ct = default);
    Task DeleteFavoriteAlbumAsync(Guid userId, Guid albumId, CancellationToken ct = default);
    Task<Guid> AddFavoriteBandAsync(Guid userId, Guid bandId, CancellationToken ct = default);
    Task DeleteFavoriteBandAsync(Guid userId, Guid bandId, CancellationToken ct = default);
    Task<(double? AverageRating, int RatingsCount)> RateAlbumAsync(Guid userId, Guid albumId, int rating, CancellationToken ct = default);
    Task<(double? AverageRating, int RatingsCount)> RateBandAsync(Guid userId, Guid bandId, int rating, CancellationToken ct = default);
    Task<(int? UserRating, double? AverageRating, int RatingsCount)> GetAlbumRatingAsync(Guid userId, Guid albumId, CancellationToken ct = default);
    Task<(double? AverageRating, int RatingsCount)> GetAlbumAverageRatingAsync(Guid albumId, CancellationToken ct = default);
    Task<(int? UserRating, double? AverageRating, int RatingsCount)> GetBandRatingAsync(Guid userId, Guid bandId, CancellationToken ct = default);
    Task<(double? AverageRating, int RatingsCount)> GetBandAverageRatingAsync(Guid bandId, CancellationToken ct = default);
    Task<PaginatedResult<AlbumCardDto>> GetTopRatedAlbumsAsync(PaginationRequest pagination, RatingPeriod period, CancellationToken ct = default);
    Task<PaginatedResult<BandCardDto>> GetTopRatedBandsAsync(PaginationRequest pagination, RatingPeriod period, CancellationToken ct = default);
}
