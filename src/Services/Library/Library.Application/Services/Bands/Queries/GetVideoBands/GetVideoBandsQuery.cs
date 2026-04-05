namespace Library.Application.Services.Bands.Queries.GetVideoBands;

public record GetVideoBandsQuery(PaginationRequest PaginationRequest)
    : BuildingBlocks.CQRS.IQuery<GetVideoBandsResult>;

public record GetVideoBandsResult(PaginatedResult<VideoBandDto> VideoBands);
