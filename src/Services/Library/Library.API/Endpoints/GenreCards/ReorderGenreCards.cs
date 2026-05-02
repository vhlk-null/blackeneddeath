namespace Library.API.Endpoints.GenreCards;

public record ReorderGenreCardsRequest(List<Guid> OrderedIds);

public class ReorderGenreCards : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/genre-cards/reorder",
                async (ReorderGenreCardsRequest request, ISender sender) =>
                {
                    await sender.Send(new ReorderGenreCardsCommand(request.OrderedIds));

                    return Results.NoContent();
                })
            .WithName("ReorderGenreCards")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Reorder Genre Cards")
            .WithDescription("Set display order for genre cards")
            .WithTags("GenreCards")
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .RequireAuthorization("AdminOnly");
    }
}
