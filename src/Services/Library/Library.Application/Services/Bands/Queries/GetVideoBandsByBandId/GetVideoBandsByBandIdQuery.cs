namespace Library.Application.Services.Bands.Queries.GetVideoBandsByBandId;

public record GetVideoBandsByBandIdQuery(Guid BandId, PaginationRequest PaginationRequest, bool ApprovedOnly = true)
    : BuildingBlocks.CQRS.IQuery<GetVideoBandsByBandIdResult>;

public record GetVideoBandsByBandIdResult(PaginatedResult<VideoBandDto> VideoBands);
