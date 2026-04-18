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
                HttpContext httpContext,
                BandSortBy sortBy = BandSortBy.Newest,
                BandStatus? status = null,
                int? yearFrom = null,
                int? yearTo = null,
                string? name = null) =>
            {
                List<Guid> genreIds   = httpContext.Request.Query["genreId"].Select(s => Guid.TryParse(s, out Guid g) ? (Guid?)g : null).Where(g => g.HasValue).Select(g => g!.Value).ToList();
                List<Guid> countryIds = httpContext.Request.Query["countryId"].Select(s => Guid.TryParse(s, out Guid g) ? (Guid?)g : null).Where(g => g.HasValue).Select(g => g!.Value).ToList();
                ISpecification<Band>? filter = BandFilterBuilder.Build(genreIds, countryIds, status, yearFrom, yearTo, name);
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
