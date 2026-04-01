namespace Library.API.Endpoints.GenreCards;

public class GetGenreCards : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/genre-cards",
                async (ISender sender) =>
                {
                    var result = await sender.Send(new GetGenreCardsQuery());

                    return Results.Ok(result.GenreCards);
                })
            .WithName("GetGenreCards")
            .Produces<IReadOnlyList<GenreCardDto>>(StatusCodes.Status200OK)
            .WithSummary("Get all GenreCards")
            .WithDescription("Get all GenreCards")
            .WithTags("GenreCards");
    }
}
