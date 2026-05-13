namespace Library.Application.Services.Albums.Queries.GetAlbumRating;

public record GetAlbumRatingQuery(Guid UserId, Guid AlbumId) : BuildingBlocks.CQRS.IQuery<GetAlbumRatingResult>;

public record GetAlbumRatingResult(Guid AlbumId, int? UserRating, double? AverageRating, int RatingsCount);
