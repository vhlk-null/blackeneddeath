namespace Library.API.Endpoints.Albums;

public record GetUpcomingAlbumsResult(PaginatedResult<AlbumCardDto> Albums);

public class GetUpcomingAlbums : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/albums/upcoming", async (
                [AsParameters] PaginationRequest paginationRequest,
                ISender sender) =>
            {
                Application.Services.Albums.Queries.GetUpcomingAlbums.GetUpcomingAlbumsResult result =
                    await sender.Send(new GetUpcomingAlbumsQuery(paginationRequest));
                return Results.Ok(result.Adapt<GetUpcomingAlbumsResult>());
            })
            .WithName("GetUpcomingAlbums")
            .Produces<GetUpcomingAlbumsResult>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Upcoming Albums")
            .WithDescription("Returns approved albums with a release year >= current year, ordered soonest first")
            .WithTags("Albums");
    }
}
