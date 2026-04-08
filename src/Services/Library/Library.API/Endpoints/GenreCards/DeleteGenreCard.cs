namespace Library.API.Endpoints.GenreCards;

public record DeleteGenreCardResponse(bool IsSuccess);

public class DeleteGenreCard : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/genre-cards/{id:guid}",
                async (Guid id, ISender sender) =>
                {
                    DeleteGenreCardResult result = await sender.Send(new DeleteGenreCardCommand(id));

                    DeleteGenreCardResponse response = result.Adapt<DeleteGenreCardResponse>();

                    return Results.Ok(response);
                })
            .WithName("DeleteGenreCard")
            .Produces<DeleteGenreCardResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Delete GenreCard")
            .WithDescription("Delete GenreCard")
            .WithTags("GenreCards");
    }
}
