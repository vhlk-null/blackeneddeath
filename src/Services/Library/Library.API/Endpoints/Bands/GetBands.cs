using BuildingBlocks.Specifications;

namespace Library.API.Endpoints.Bands;

public record GetBandsResult(PaginatedResult<BandCardDto> Bands);

public class GetBands : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/bands", async (
                [AsParameters] PaginationRequest paginationRequest,
                ISender sender,
                ILibraryDbContext db,
                CancellationToken ct,
                HttpContext httpContext,
                BandSortBy sortBy = BandSortBy.Newest,
                BandStatus? status = null,
                int? yearFrom = null,
                int? yearTo = null,
                string? name = null) =>
            {
                List<string> genreNames   = httpContext.Request.Query["genreName"].Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s!).ToList();
                List<string> countryNames = httpContext.Request.Query["countryName"].Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s!).ToList();
                List<Guid>   genreIds     = httpContext.Request.Query["genreId"].Select(s => Guid.TryParse(s, out Guid g) ? (Guid?)g : null).Where(g => g.HasValue).Select(g => g!.Value).ToList();
                List<Guid>   countryIds   = httpContext.Request.Query["countryId"].Select(s => Guid.TryParse(s, out Guid g) ? (Guid?)g : null).Where(g => g.HasValue).Select(g => g!.Value).ToList();

                ISpecification<Band>? filter;
                if (genreNames.Count > 0 || countryNames.Count > 0)
                    filter = await BandFilterBuilder.BuildByNameAsync(db, genreNames, countryNames, status, yearFrom, yearTo, name, ct);
                else
                    filter = BandFilterBuilder.Build(genreIds, countryIds, status, yearFrom, yearTo, name);

                Application.Services.Bands.Queries.GetBands.GetBandsResult result = await sender.Send(new GetBandsQuery(paginationRequest, sortBy, filter));
                return Results.Ok(result.Adapt<GetBandsResult>());
            })
            .WithName("GetBands")
            .Produces<GetBandsResult>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Bands")
            .WithDescription("Get Bands with optional multi-value filters: genreId[], countryId[], genreName[], countryName[], status, yearFrom, yearTo, name")
            .WithTags("Bands");
    }
}
