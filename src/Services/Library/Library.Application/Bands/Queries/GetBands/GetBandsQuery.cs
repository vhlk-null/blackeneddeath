namespace Library.Application.Bands.Queries.GetBands;

public record GetBandsQuery : BuildingBlocks.CQRS.IQuery<GetBandsResult>;
public record GetBandsResult(IEnumerable<BandDto> Bands);
