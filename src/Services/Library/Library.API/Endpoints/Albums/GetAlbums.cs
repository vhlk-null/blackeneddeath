using BuildingBlocks.Specifications;

namespace Library.API.Endpoints.Albums;

public record GetAlbumsResult(PaginatedResult<AlbumCardDto> Albums);
public class GetAlbums : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/albums", async (
                [AsParameters] PaginationRequest paginationRequest,
                ISender sender,
                ILibraryDbContext db,
                CancellationToken ct,
                HttpContext httpContext,
                AlbumSortBy sortBy = AlbumSortBy.ReleaseDate,
                int? year = null,
                int? yearFrom = null,
                int? yearTo = null,
                string? name = null) =>
            {
                SortDir sortDir = Enum.TryParse<SortDir>(httpContext.Request.Query["sortDir"], ignoreCase: true, out SortDir sd) ? sd : SortDir.Desc;

                List<string> genreNames   = httpContext.Request.Query["genreName"].Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s!).ToList();
                List<string> labelNames   = httpContext.Request.Query["labelName"].Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s!).ToList();
                List<string> countryNames = httpContext.Request.Query["countryName"].Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s!).ToList();
                List<Guid>   genreIds     = httpContext.Request.Query["genreId"].Select(s => Guid.TryParse(s, out Guid g) ? (Guid?)g : null).Where(g => g.HasValue).Select(g => g!.Value).ToList();
                List<Guid>   labelIds     = httpContext.Request.Query["labelId"].Select(s => Guid.TryParse(s, out Guid g) ? (Guid?)g : null).Where(g => g.HasValue).Select(g => g!.Value).ToList();
                List<Guid>   countryIds   = httpContext.Request.Query["countryId"].Select(s => Guid.TryParse(s, out Guid g) ? (Guid?)g : null).Where(g => g.HasValue).Select(g => g!.Value).ToList();
                List<AlbumType> types     = httpContext.Request.Query["type"].Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => Enum.TryParse<AlbumType>(s, true, out AlbumType t) ? (AlbumType?)t : null).Where(t => t.HasValue).Select(t => t!.Value).ToList();

                int? resolvedYearFrom = year ?? yearFrom;
                int? resolvedYearTo   = year ?? yearTo;

                ISpecification<Album>? filter;
                if (genreNames.Count > 0 || labelNames.Count > 0 || countryNames.Count > 0)
                    filter = await AlbumFilterBuilder.BuildByNameAsync(db, genreNames, labelNames, countryNames, types, resolvedYearFrom, resolvedYearTo, name, ct);
                else
                    filter = AlbumFilterBuilder.Build(genreIds, labelIds, countryIds, types, resolvedYearFrom, resolvedYearTo, name);

                Application.Services.Albums.Queries.GetAlbums.GetAlbumsResult result = await sender.Send(new GetAlbumsQuery(paginationRequest, sortBy, sortDir, filter));
                return Results.Ok(result.Adapt<GetAlbumsResult>());
            })
            .WithName("GetAlbums")
            .Produces<GetAlbumsResult>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Albums")
            .WithDescription("Get Albums with optional multi-value filters: genreId[], labelId[], countryId[], genreName[], labelName[], countryName[], type, year, yearFrom, yearTo, name")
            .WithTags("Albums");
    }
}
