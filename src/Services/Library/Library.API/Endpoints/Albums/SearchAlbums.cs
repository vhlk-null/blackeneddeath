namespace Library.API.Endpoints.Albums;

public record SearchAlbumsResponse(PaginatedResult<AlbumSearchDocument> Albums);

public class SearchAlbums : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/albums/search", async (
                string q,
                ISender sender,
                HttpContext httpContext,
                CancellationToken ct,
                int pageIndex = 0,
                int pageSize = 20,
                string sortBy = "createdAt",
                SortDir sortDir = SortDir.Desc,
                string? type = null,
                int? releaseYearFrom = null,
                int? releaseYearTo = null) =>
            {
                List<string> genres = httpContext.Request.Query["genre"]
                    .Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s!).ToList();
                List<string> countries = httpContext.Request.Query["country"]
                    .Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s!).ToList();

                Application.Services.Albums.Queries.SearchAlbums.SearchAlbumsResult result =
                    await sender.Send(new SearchAlbumsQuery(
                        q, pageIndex, pageSize,
                        genres.Count > 0 ? genres : null,
                        countries.Count > 0 ? countries : null,
                        type, releaseYearFrom, releaseYearTo,
                        sortBy, sortDir), ct);

                return Results.Ok(new SearchAlbumsResponse(result.Albums));
            })
            .WithName("SearchAlbums")
            .Produces<SearchAlbumsResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Search Albums")
            .WithDescription("Full-text search albums via Meilisearch. Multi-value: genre[], country[]")
            .WithTags("Albums");
    }
}
