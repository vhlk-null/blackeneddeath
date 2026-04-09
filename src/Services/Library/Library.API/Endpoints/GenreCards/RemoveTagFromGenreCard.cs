namespace Library.API.Endpoints.GenreCards;

public record RemoveTagFromGenreCardResponse(bool IsSuccess);

public class RemoveTagFromGenreCard : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/genre-cards/{id:guid}/tags/{tagId:guid}",
                async (Guid id, Guid tagId, ISender sender) =>
                {
                    RemoveTagFromGenreCardResult result = await sender.Send(new RemoveTagFromGenreCardCommand(id, tagId));

                    return Results.Ok(result.Adapt<RemoveTagFromGenreCardResponse>());
                })
            .WithName("RemoveTagFromGenreCard")
            .Produces<RemoveTagFromGenreCardResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Remove Tag from GenreCard")
            .WithDescription("Remove Tag from GenreCard")
            .WithTags("GenreCards")
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .RequireAuthorization("AdminOnly");
    }
}
