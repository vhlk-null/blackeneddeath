namespace Library.API.Endpoints.Bands;

public record GetBandsResult(PaginatedResult<BandCardDto> Bands);

public class GetBands : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/bands", async (
                [AsParameters] PaginationRequest paginationRequest,
                ISender sender,
                HttpContext httpContext,
                BandSortBy sortBy = BandSortBy.FormedYear) =>
            {
                SortDir sortDir = Enum.TryParse<SortDir>(httpContext.Request.Query["sortDir"], ignoreCase: true, out SortDir sd) ? sd : SortDir.Desc;

                Application.Services.Bands.Queries.GetBands.GetBandsResult result = await sender.Send(new GetBandsQuery(paginationRequest, sortBy, sortDir));
                return Results.Ok(result.Adapt<GetBandsResult>());
            })
            .WithName("GetBands")
            .Produces<GetBandsResult>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Bands")
            .WithDescription("Get Bands")
            .WithTags("Bands");
    }
}
