namespace Library.Application.Services.Bands.Queries.GetBands;

public enum BandSortBy { Newest, Oldest, Name, FormedYear }

public record GetBandsQuery(
    PaginationRequest PaginationRequest,
    BandSortBy SortBy = BandSortBy.Newest,
    ISpecification<Band>? Filter = null,
    bool ApprovedOnly = true) : BuildingBlocks.CQRS.IQuery<GetBandsResult>;

public record GetBandsResult(PaginatedResult<BandCardDto> Bands);
