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
                BandSortBy sortBy = BandSortBy.Newest,
                Guid? genreId = null,
                Guid? countryId = null,
                BandStatus? status = null,
                int? yearFrom = null,
                int? yearTo = null,
                string? name = null,
                string? genreName = null,
                string? countryName = null) =>
            {
                ISpecification<Band>? filter;
                if (genreName != null || countryName != null)
                    filter = await BandFilterBuilder.BuildByNameAsync(db, genreName, countryName, status, yearFrom, yearTo, name, ct);
                else
                    filter = BandFilterBuilder.Build(genreId, countryId, status, yearFrom, yearTo, name);

                Application.Services.Bands.Queries.GetBands.GetBandsResult result = await sender.Send(new GetBandsQuery(paginationRequest, sortBy, filter));
                return Results.Ok(result.Adapt<GetBandsResult>());
            })
            .WithName("GetBands")
            .Produces<GetBandsResult>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Bands")
            .WithDescription("Get Bands with optional filters: genreId, countryId, status, yearFrom, yearTo, genreName, countryName")
            .WithTags("Bands");
    }
}
