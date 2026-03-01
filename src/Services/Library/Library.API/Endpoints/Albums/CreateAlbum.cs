namespace Library.API.Endpoints.Albums;

public record CreateAlbumRequest(AlbumDto Album);

public record CreateAlbumResponse(Guid Id);

public class CreateAlbum : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/albums",
                async (CreateAlbumRequest request, ISender sender) =>
                {
                    var command = request.Adapt<CreateAlbumCommand>();

                    var result = await sender.Send(command);

                    var response = result.Adapt<CreateAlbumResponse>();

                    return Results.Created($"/albums/{response.Id}", response);
                })
            .WithName("CreatedAlbum")
            .Produces<CreateAlbumResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Album")
            .WithDescription("Create Album");
    }
}