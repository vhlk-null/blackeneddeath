namespace Library.Application.Services.Bands.Queries.GetBandRating;

public record GetBandRatingQuery(Guid UserId, Guid BandId) : BuildingBlocks.CQRS.IQuery<GetBandRatingResult>;

public record GetBandRatingResult(Guid BandId, int? UserRating, double? AverageRating, int RatingsCount);
