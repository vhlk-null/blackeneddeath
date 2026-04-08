namespace Library.API.Endpoints.GenreCards;

public record AddGenreToGenreCardResponse(bool IsSuccess);

public class AddGenreToGenreCard : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/genre-cards/{id:guid}/genres/{genreId:guid}",
                async (Guid id, Guid genreId, ISender sender) =>
                {
                    AddGenreToGenreCardResult result = await sender.Send(new AddGenreToGenreCardCommand(id, genreId));

                    return Results.Ok(result.Adapt<AddGenreToGenreCardResponse>());
                })
            .WithName("AddGenreToGenreCard")
            .Produces<AddGenreToGenreCardResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Add Genre to GenreCard")
            .WithDescription("Add Genre to GenreCard")
            .WithTags("GenreCards");
    }
}
