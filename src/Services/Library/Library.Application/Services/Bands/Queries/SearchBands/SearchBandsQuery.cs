namespace Library.Application.Services.Bands.Queries.SearchBands;

public record SearchBandsQuery(
    string Q,
    int PageIndex = 0,
    int PageSize = 20,
    List<string>? Genres = null,
    List<string>? Countries = null,
    string? Status = null,
    int? FormedYearFrom = null,
    int? FormedYearTo = null,
    string SortBy = "createdAt",
    SortDir SortDir = SortDir.Desc) : BuildingBlocks.CQRS.IQuery<SearchBandsResult>;

public record SearchBandsResult(PaginatedResult<BandSearchDocument> Bands);
