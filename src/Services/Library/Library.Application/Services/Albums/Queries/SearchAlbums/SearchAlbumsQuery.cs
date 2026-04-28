namespace Library.Application.Services.Albums.Queries.SearchAlbums;

public record SearchAlbumsQuery(
    string Q,
    int PageIndex = 0,
    int PageSize = 20,
    List<string>? Genres = null,
    List<string>? Countries = null,
    string? Type = null,
    int? ReleaseYearFrom = null,
    int? ReleaseYearTo = null,
    string SortBy = "createdAt",
    SortDir SortDir = SortDir.Desc,
    bool IncludeTracks = false) : BuildingBlocks.CQRS.IQuery<SearchAlbumsResult>;

public record SearchAlbumsResult(PaginatedResult<AlbumSearchDocument> Albums);