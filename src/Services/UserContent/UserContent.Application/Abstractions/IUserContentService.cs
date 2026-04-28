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

    Task<PaginatedResult<CommentDto>> GetAlbumCommentsAsync(Guid albumId, int pageIndex, int pageSize, Guid? requestingUserId = null, CancellationToken ct = default);
    Task<CommentDto> CreateAlbumCommentAsync(CreateAlbumCommentRequest request, CancellationToken ct = default);
    Task<CommentDto> UpdateAlbumCommentAsync(Guid commentId, UpdateCommentRequest request, CancellationToken ct = default);
    Task DeleteAlbumCommentAsync(Guid commentId, CancellationToken ct = default);
    Task<CommentDto> ReactToAlbumCommentAsync(Guid commentId, ReactToCommentRequest request, CancellationToken ct = default);
    Task DeleteAlbumCommentReactionAsync(Guid commentId, Guid userId, CancellationToken ct = default);

    Task<PaginatedResult<CommentDto>> GetBandCommentsAsync(Guid bandId, int pageIndex, int pageSize, Guid? requestingUserId = null, CancellationToken ct = default);
    Task<CommentDto> CreateBandCommentAsync(CreateBandCommentRequest request, CancellationToken ct = default);
    Task<CommentDto> UpdateBandCommentAsync(Guid commentId, UpdateCommentRequest request, CancellationToken ct = default);
    Task DeleteBandCommentAsync(Guid commentId, CancellationToken ct = default);
    Task<CommentDto> ReactToBandCommentAsync(Guid commentId, ReactToCommentRequest request, CancellationToken ct = default);
    Task DeleteBandCommentReactionAsync(Guid commentId, Guid userId, CancellationToken ct = default);
}
