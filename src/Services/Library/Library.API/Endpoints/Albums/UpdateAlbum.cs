namespace Library.API.Endpoints.Albums;

public record UpdateAlbumRequest(AlbumDto Album);

public record UpdateAlbumResponse(bool IsSuccess);

public class UpdateAlbum : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/albums", async (UpdateAlbumRequest request, ISender sender) =>
            {
                var command = request.Adapt<UpdateAlbumCommand>();

                var result = await sender.Send(command);

                var response = result.Adapt<UpdateAlbumResponse>();

                return Results.Ok(response);
            })
            .WithName("UpdateAlbums")
            .Produces<UpdateAlbumResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Update Albums")
            .WithDescription("Update Albums")
            .WithTags("Albums");
    }
}