using Library.Application.Services.Bands.Specifications;

namespace Library.API.Endpoints.Bands;

public record GetBandsResult(PaginatedResult<BandCardDto> Bands);

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
                BandStatus? status = null,
                int? formedYear = null) =>
            {
                var filter = BandFilterBuilder.Build(genreId, countryId, status, formedYear);
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
}
