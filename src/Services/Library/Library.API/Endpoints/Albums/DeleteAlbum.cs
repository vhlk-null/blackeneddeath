namespace Library.API.Endpoints.Albums;

//public record DeleteAlbumRequest(Guid Id);
public record DeleteAlbumResponse(bool IsSuccess);

public class DeleteAlbum : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/albums/{id:guid}", async (Guid id, ISender sender) =>
            {
                var command = new DeleteAlbumCommand(id);
                var result = await sender.Send(command);
                var response = result.Adapt<DeleteAlbumResponse>();
                return Results.Ok(response);
            })
            .WithName("DeleteAlbum")
            .Produces<DeleteAlbumResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Delete Album")
            .WithDescription("Delete Album")
            .WithTags("Albums");
    }
}