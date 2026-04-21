namespace UserContent.Application.Abstractions;

public interface IUserContentService
{
    Task<UserProfileDto> GetUserProfileAsync(Guid userId, CancellationToken ct = default);
    Task<PaginatedResult<AlbumCardDto>> GetFavoriteAlbumsAsync(Guid userId, int pageIndex, int pageSize, CancellationToken ct = default);
    Task<bool> IsAlbumFavoriteAsync(Guid userId, Guid albumId, CancellationToken ct = default);
    Task<Guid> AddFavoriteAlbumAsync(Guid userId, Guid albumId, CancellationToken ct = default);
    Task DeleteFavoriteAlbumAsync(Guid userId, Guid albumId, CancellationToken ct = default);
    Task<PaginatedResult<BandCardDto>> GetFavoriteBandsAsync(Guid userId, int pageIndex, int pageSize, CancellationToken ct = default);
    Task<bool> IsBandFavoriteAsync(Guid userId, Guid bandId, CancellationToken ct = default);
    Task<Guid> AddFavoriteBandAsync(Guid userId, Guid bandId, CancellationToken ct = default);
    Task DeleteFavoriteBandAsync(Guid userId, Guid bandId, CancellationToken ct = default);
    Task<(double? AverageRating, int RatingsCount, bool IsExplicit)> RateAlbumAsync(Guid userId, Guid albumId, int rating, CancellationToken ct = default);
    Task<(double? AverageRating, int RatingsCount)> RateBandAsync(Guid userId, Guid bandId, int rating, CancellationToken ct = default);
    Task<(int? UserRating, double? AverageRating, int RatingsCount, bool IsExplicit)> GetAlbumRatingAsync(Guid userId, Guid albumId, CancellationToken ct = default);
    Task<(double? AverageRating, int RatingsCount, bool IsExplicit)> GetAlbumAverageRatingAsync(Guid albumId, CancellationToken ct = default);
    Task<(int? UserRating, double? AverageRating, int RatingsCount)> GetBandRatingAsync(Guid userId, Guid bandId, CancellationToken ct = default);
    Task<(double? AverageRating, int RatingsCount)> GetBandAverageRatingAsync(Guid bandId, CancellationToken ct = default);
    Task<PaginatedResult<AlbumCardDto>> GetTopRatedAlbumsAsync(PaginationRequest pagination, RatingPeriod period, SortDir sortDir = SortDir.Desc, CancellationToken ct = default);
    Task<PaginatedResult<BandCardDto>> GetTopRatedBandsAsync(PaginationRequest pagination, RatingPeriod period, SortDir sortDir = SortDir.Desc, CancellationToken ct = default);

    Task<PaginatedResult<ReviewDto>> GetBandAlbumReviewsAsync(Guid bandId, int pageIndex, int pageSize, ReviewOrderBy orderBy, CancellationToken ct = default);
    Task<PaginatedResult<ReviewDto>> GetAlbumReviewsAsync(Guid albumId, int pageIndex, int pageSize, ReviewOrderBy orderBy, CancellationToken ct = default);
    Task<int> GetAlbumReviewCountAsync(Guid albumId, CancellationToken ct = default);
    Task<ReviewDto> CreateAlbumReviewAsync(CreateAlbumReviewRequest request, CancellationToken ct = default);
    Task<ReviewDto> UpdateAlbumReviewAsync(Guid reviewId, UpdateReviewRequest request, CancellationToken ct = default);
    Task DeleteAlbumReviewAsync(Guid reviewId, CancellationToken ct = default);

    Task<PaginatedResult<ReviewDto>> GetBandReviewsAsync(Guid bandId, int pageIndex, int pageSize, ReviewOrderBy orderBy, CancellationToken ct = default);
    Task<int> GetBandReviewCountAsync(Guid bandId, CancellationToken ct = default);
    Task<ReviewDto> CreateBandReviewAsync(CreateBandReviewRequest request, CancellationToken ct = default);
    Task<ReviewDto> UpdateBandReviewAsync(Guid reviewId, UpdateReviewRequest request, CancellationToken ct = default);
    Task DeleteBandReviewAsync(Guid reviewId, CancellationToken ct = default);

    Task<List<CollectionSummaryDto>> GetUserCollectionsAsync(Guid userId, CancellationToken ct = default);
    Task<CollectionDetailDto> GetCollectionAsync(Guid collectionId, CancellationToken ct = default);
    Task<CollectionDto> CreateCollectionAsync(CreateCollectionRequest request, Stream? coverImage, string? coverContentType, string? coverFileName, CancellationToken ct = default);
    Task<CollectionDto> UpdateCollectionAsync(Guid collectionId, UpdateCollectionRequest request, CancellationToken ct = default);
    Task<CollectionDto> UpdateCollectionCoverAsync(Guid collectionId, Stream coverImage, string coverContentType, string coverFileName, CancellationToken ct = default);
    Task DeleteCollectionAsync(Guid collectionId, CancellationToken ct = default);
    Task AddAlbumToCollectionAsync(Guid collectionId, Guid albumId, CancellationToken ct = default);
    Task RemoveAlbumFromCollectionAsync(Guid collectionId, Guid albumId, CancellationToken ct = default);
    Task AddBandToCollectionAsync(Guid collectionId, Guid bandId, CancellationToken ct = default);
    Task RemoveBandFromCollectionAsync(Guid collectionId, Guid bandId, CancellationToken ct = default);
}
