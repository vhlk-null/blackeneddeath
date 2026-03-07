namespace Library.Application.Services.Bands.Queries.GetBands;

public record GetBandsQuery(PaginationRequest PaginationRequest) : BuildingBlocks.CQRS.IQuery<GetBandsResult>;
public record GetBandsResult(PaginatedResult<BandDto> Bands);
