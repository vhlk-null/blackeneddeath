namespace Library.Application.Services.Bands.Commands.RateBand;

public record RateBandCommand(Guid UserId, Guid BandId, int Rating) : BuildingBlocks.CQRS.ICommand<RateBandResult>;

public record RateBandResult(Guid BandId, int UserRating, double? AverageRating, int RatingsCount);
