namespace Library.API.Endpoints.GenreCards;

public class GetGenreDetailsCards : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/genre-cards-details",
                async (ISender sender) =>
                {
                    GetGenreDetailsCardsResult result = await sender.Send(new GetGenreDetailsCardsQuery());

                    return Results.Ok(result.GenreCards);
                })
            .WithName("GetGenreDetailsCards")
            .Produces<IReadOnlyList<GenreCardDetailDto>>(StatusCodes.Status200OK)
            .WithSummary("Get all GenreDetailsCards")
            .WithDescription("Get all GenreDetailsCards")
            .WithTags("GenreCards");
    }
}