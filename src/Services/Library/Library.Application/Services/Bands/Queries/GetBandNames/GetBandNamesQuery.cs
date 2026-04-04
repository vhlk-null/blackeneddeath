namespace Library.Application.Services.Bands.Queries.GetBandNames;

public record GetBandNamesQuery() : BuildingBlocks.CQRS.IQuery<GetBandNamesResult>;
public record GetBandNamesResult(List<NameIdDto> Bands);
