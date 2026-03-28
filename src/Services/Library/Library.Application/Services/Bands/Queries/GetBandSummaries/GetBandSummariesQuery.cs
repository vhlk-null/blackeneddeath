namespace Library.Application.Services.Bands.Queries.GetBandSummaries;

public record GetBandSummariesQuery : BuildingBlocks.CQRS.IQuery<GetBandSummariesResult>;
public record GetBandSummariesResult(List<BandSummaryDto> Bands);
