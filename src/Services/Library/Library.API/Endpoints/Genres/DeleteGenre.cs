using Library.Application.Genres.Commands.DeleteGenre;

namespace Library.API.Endpoints.Genres;

public record DeleteGenreResponse(bool IsSuccess);

public class DeleteGenre : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/genres/{id:guid}",
                async (Guid id, ISender sender) =>
                {
                    var command = new DeleteGenreCommand(id);

                    var result = await sender.Send(command);

                    return Results.Ok(result.Adapt<DeleteGenreResponse>());
                })
            .WithName("DeleteGenre")
            .Produces<DeleteGenreResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Delete Genre")
            .WithDescription("Delete Genre");
    }
}
