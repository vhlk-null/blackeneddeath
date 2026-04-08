namespace Library.API.Endpoints.Albums;

public record GetAlbumByIdResponse(AlbumDetailDto Album);

public class GetAlbumById : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/albums/{id:guid}", async (Guid id, ISender sender) =>
            {
                GetAlbumByIdQuery query = new GetAlbumByIdQuery(id);
                GetAlbumByIdResult result = await sender.Send(query);
                GetAlbumByIdResponse response = result.Adapt<GetAlbumByIdResponse>();
                return Results.Ok(response);
            })
            .WithName("GetAlbumById")
            .Produces<GetAlbumByIdResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Album by Id")
            .WithDescription("Get Album by Id")
            .WithTags("Albums");
    }
}