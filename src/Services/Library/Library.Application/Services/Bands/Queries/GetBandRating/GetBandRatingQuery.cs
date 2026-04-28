namespace Library.Application.Services.Bands.Queries.GetBandRating;

public record GetBandRatingQuery(Guid BandId, Guid? UserId = null) : BuildingBlocks.CQRS.IQuery<GetBandRatingResult>;

public record GetBandRatingResult(Guid BandId, int? UserRating, double? AverageRating, int RatingsCount);
