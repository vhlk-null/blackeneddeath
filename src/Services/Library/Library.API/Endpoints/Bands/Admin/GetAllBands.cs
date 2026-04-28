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
                BandSortBy sortBy = BandSortBy.FormedYear,
                BandStatus? status = null) =>
            {
                SortDir sortDir = Enum.TryParse<SortDir>(httpContext.Request.Query["sortDir"], ignoreCase: true, out SortDir sd) ? sd : SortDir.Desc;

                Application.Services.Bands.Queries.GetBands.GetBandsResult result = await sender.Send(new GetBandsQuery(paginationRequest, sortBy, sortDir, ApprovedOnly: false));
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
