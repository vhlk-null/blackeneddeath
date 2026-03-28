namespace Library.API.Endpoints.Bands;

public record GetBandsResult(PaginatedResult<BandDto> Bands);
public class GetBands : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/bands", async ([AsParameters] PaginationRequest paginationRequest, ISender sender, BandSortBy sortBy = BandSortBy.Newest) =>
            {
                var query = new GetBandsQuery(paginationRequest, sortBy);
                var result = await sender.Send(query);
                var response = result.Adapt<GetBandsResult>();
                return Results.Ok(response);
            })
            .WithName("GetBands")
            .Produces<GetBandsResult>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Bands")
            .WithDescription("Get Bands")
            .WithTags("Bands");
    }
}