namespace Library.API.Endpoints.Genres;

public record UpdateGenreRequest(string Name, Guid? ParentGenreId);

public record UpdateGenreResponse(bool IsSuccess);

public class UpdateGenre : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/genres/{id}",
                async (Guid id, UpdateGenreRequest request, ISender sender) =>
                {
                    var command = request.Adapt<UpdateGenreCommand>() with { Id = id };

                    var result = await sender.Send(command);

                    return Results.Ok(result.Adapt<UpdateGenreResponse>());
                })
            .WithName("UpdateGenre")
            .Produces<UpdateGenreResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update Genre")
            .WithDescription("Update Genre");
    }
}
