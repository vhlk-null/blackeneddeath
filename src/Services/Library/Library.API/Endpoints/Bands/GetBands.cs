using BuildingBlocks.Specifications;
using Library.Application.Services.Bands.Specifications;

namespace Library.API.Endpoints.Bands;

public record GetBandsResult(PaginatedResult<BandDto> Bands);

public class GetBands : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/bands", async (
                [AsParameters] PaginationRequest paginationRequest,
                ISender sender,
                BandSortBy sortBy = BandSortBy.Newest,
                Guid? genreId = null,
                Guid? countryId = null,
                BandStatus? status = null) =>
            {
                ISpecification<Band>? filter = null;

                if (genreId.HasValue)   filter = Combine(filter, new BandByGenreSpec(genreId.Value));
                if (countryId.HasValue) filter = Combine(filter, new BandByCountrySpec(countryId.Value));
                if (status.HasValue)    filter = Combine(filter, new BandByStatusSpec(status.Value));

                var result = await sender.Send(new GetBandsQuery(paginationRequest, sortBy, filter));
                return Results.Ok(result.Adapt<GetBandsResult>());
            })
            .WithName("GetBands")
            .Produces<GetBandsResult>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Bands")
            .WithDescription("Get Bands with optional filters: genreId, countryId, status")
            .WithTags("Bands");
    }

    private static ISpecification<Band> Combine(ISpecification<Band>? current, ISpecification<Band> next) =>
        current is null ? next : current.And(next);
}
