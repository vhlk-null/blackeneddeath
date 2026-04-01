namespace Library.API.Endpoints.GenreCards;

public record AddTagToGenreCardResponse(bool IsSuccess);

public class AddTagToGenreCard : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/genre-cards/{id:guid}/tags/{tagId:guid}",
                async (Guid id, Guid tagId, ISender sender) =>
                {
                    var result = await sender.Send(new AddTagToGenreCardCommand(id, tagId));

                    return Results.Ok(result.Adapt<AddTagToGenreCardResponse>());
                })
            .WithName("AddTagToGenreCard")
            .Produces<AddTagToGenreCardResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Add Tag to GenreCard")
            .WithDescription("Add Tag to GenreCard")
            .WithTags("GenreCards");
    }
}
