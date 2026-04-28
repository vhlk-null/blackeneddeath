namespace Library.Application.Services.Albums.Queries.GetAlbumRating;

public record GetAlbumRatingQuery(Guid AlbumId, Guid? UserId = null) : BuildingBlocks.CQRS.IQuery<GetAlbumRatingResult>;

public record GetAlbumRatingResult(Guid AlbumId, int? UserRating, double? AverageRating, int RatingsCount);
