using Meilisearch;

namespace Library.Application.Services.Bands.Queries.SearchBands;

public class SearchBandsQueryHandler(MeilisearchClient client)
    : BuildingBlocks.CQRS.IQueryHandler<SearchBandsQuery, SearchBandsResult>
{
    public async ValueTask<SearchBandsResult> Handle(SearchBandsQuery query, CancellationToken cancellationToken)
    {
        string sortDir = query.SortDir == SortDir.Desc ? "desc" : "asc";

        var searchQuery = new SearchQuery
        {
            Page = query.PageIndex + 1,
            HitsPerPage = query.PageSize,
            Filter = BandSearchFilterBuilder.Build(query.Genres, query.Countries, query.Status, query.FormedYearFrom, query.FormedYearTo),
            Sort = [$"{query.SortBy}:{sortDir}"]
        };

        PaginatedSearchResult<BandSearchDocument> result = (PaginatedSearchResult<BandSearchDocument>)await client
            .Index(SearchIndexes.Bands)
            .SearchAsync<BandSearchDocument>(query.Q, searchQuery, cancellationToken);

        return new SearchBandsResult(
            new PaginatedResult<BandSearchDocument>(query.PageIndex, query.PageSize, result.TotalHits, result.Hits));
    }
}
