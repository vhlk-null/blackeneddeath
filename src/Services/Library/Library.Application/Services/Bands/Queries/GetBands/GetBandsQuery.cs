namespace Library.Application.Services.Bands.Queries.GetBands;

public enum BandSortBy { FormedYear, Name, CreatedAt }

public record GetBandsQuery(
    PaginationRequest PaginationRequest,
    BandSortBy SortBy = BandSortBy.FormedYear,
    SortDir SortDir = SortDir.Desc,
    ISpecification<Band>? Filter = null,
    bool ApprovedOnly = true) : BuildingBlocks.CQRS.IQuery<GetBandsResult>;

public record GetBandsResult(PaginatedResult<BandCardDto> Bands);
