namespace Library.Application.Services.Bands.Queries.GetVideoBandsByBandId;

public record GetVideoBandsByBandIdQuery(Guid BandId) : BuildingBlocks.CQRS.IQuery<GetVideoBandsByBandIdResult>;

public record GetVideoBandsByBandIdResult(IReadOnlyList<VideoBandDto> VideoBands);
