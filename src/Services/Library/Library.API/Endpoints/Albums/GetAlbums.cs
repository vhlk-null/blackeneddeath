namespace Library.API.Endpoints.Albums;

public record GetAlbumsResult(PaginatedResult<AlbumDto> Albums);
public class GetAlbums : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/albums", async ([AsParameters] PaginationRequest paginationRequest, ISender sender) =>
            {
                var query = new GetAlbumsQuery(paginationRequest);
                var result = await sender.Send(query);
                var response = result.Adapt<GetAlbumsResult>();
                return Results.Ok(response);
            })
            .WithName("GetAlbums")
            .Produces<GetAlbumsResult>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Albums")
            .WithDescription("Get Albums")
            .WithTags("Albums");
    }
}
