using CloudinaryDotNet.Actions;
using Meilisearch;

namespace Library.Application.Services.Albums.Queries.SearchAlbums;

public class SearchAlbumsQueryHandler(MeilisearchClient client)
    : BuildingBlocks.CQRS.IQueryHandler<SearchAlbumsQuery, SearchAlbumsResult>
{
    public async ValueTask<SearchAlbumsResult> Handle(SearchAlbumsQuery query, CancellationToken cancellationToken)
    {
        string sortDir = query.SortDir == SortDir.Desc ? "desc" : "asc";

        var searchQuery = new SearchQuery
        {
            Page = query.PageIndex + 1,
            HitsPerPage = query.PageSize,
            Filter = AlbumSearchFilterBuilder.Build(
                query.Genres,
                query.Countries,
                query.Type,
                query.ReleaseYearFrom,
                query.ReleaseYearTo),
            Sort = [$"{query.SortBy}:{sortDir}"]
        };

        PaginatedSearchResult<AlbumSearchDocument> result = (PaginatedSearchResult<AlbumSearchDocument>)await client
            .Index(SearchIndexes.Albums)
            .SearchAsync<AlbumSearchDocument>(query.Q, searchQuery, cancellationToken);

        return new SearchAlbumsResult(
            new PaginatedResult<AlbumSearchDocument>(
                query.PageIndex,
                query.PageSize,
                result.TotalHits,
                result.Hits));
    }
}