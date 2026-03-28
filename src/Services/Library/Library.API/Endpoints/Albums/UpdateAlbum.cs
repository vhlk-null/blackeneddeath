namespace Library.API.Endpoints.Albums;

public record UpdateAlbumResponse(bool IsSuccess);

public class UpdateAlbum : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/albums", async (UpdateAlbumDto request, ISender sender) =>
            {
                var command = new UpdateAlbumCommand(request);
                var result = await sender.Send(command);
                return Results.Ok(result.Adapt<UpdateAlbumResponse>());
            })
            .WithName("UpdateAlbum")
            .Produces<UpdateAlbumResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Update Album")
            .WithDescription("Update Album")
            .WithTags("Albums");
    }
}
