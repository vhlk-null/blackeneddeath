namespace Library.API.Endpoints.Bands;

public record SearchBandsResponse(PaginatedResult<BandSearchDocument> Bands);

public class SearchBands : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/bands/search", async (
                string q,
                ISender sender,
                HttpContext httpContext,
                CancellationToken ct,
                int pageIndex = 0,
                int pageSize = 20,
                string sortBy = "createdAt",
                SortDir sortDir = SortDir.Desc,
                string? status = null,
                int? formedYearFrom = null,
                int? formedYearTo = null) =>
            {
                List<string> genres = httpContext.Request.Query["genre"]
                    .Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s!).ToList();
                List<string> countries = httpContext.Request.Query["country"]
                    .Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s!).ToList();

                Application.Services.Bands.Queries.SearchBands.SearchBandsResult result =
                    await sender.Send(new SearchBandsQuery(
                        q, pageIndex, pageSize,
                        genres.Count > 0 ? genres : null,
                        countries.Count > 0 ? countries : null,
                        status, formedYearFrom, formedYearTo,
                        sortBy, sortDir), ct);

                return Results.Ok(new SearchBandsResponse(result.Bands));
            })
            .WithName("SearchBands")
            .Produces<SearchBandsResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Search Bands")
            .WithDescription("Full-text search bands via Meilisearch. Multi-value: genre[], country[]")
            .WithTags("Bands");
    }
}
