using BuildingBlocks.Specifications;

namespace Library.API.Endpoints.Bands.Admin;

public record GetAllBandsResult(PaginatedResult<BandCardDto> Bands);

public class GetAllBands : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/admin/bands", async (
                [AsParameters] PaginationRequest paginationRequest,
                ISender sender,
                BandSortBy sortBy = BandSortBy.Newest,
                Guid? genreId = null,
                Guid? countryId = null,
                BandStatus? status = null,
                int? formedYear = null) =>
            {
                ISpecification<Band>? filter = BandFilterBuilder.Build(genreId, countryId, status, formedYear);
                Application.Services.Bands.Queries.GetBands.GetBandsResult result = await sender.Send(new GetBandsQuery(paginationRequest, sortBy, filter, ApprovedOnly: false));
                return Results.Ok(result.Adapt<GetAllBandsResult>());
            })
            .WithName("AdminGetBands")
            .Produces<GetAllBandsResult>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Admin: Get All Bands")
            .WithDescription("Get all bands including unapproved")
            .WithTags("Admin")
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .RequireAuthorization("AdminOnly");
    }
}
