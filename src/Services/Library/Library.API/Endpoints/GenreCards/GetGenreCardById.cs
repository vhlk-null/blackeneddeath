namespace Library.API.Endpoints.GenreCards;

public class GetGenreCardById : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/genre-cards/{id:guid}",
                async (Guid id, ISender sender) =>
                {
                    var result = await sender.Send(new GetGenreCardByIdQuery(id));

                    return Results.Ok(result.GenreCard);
                })
            .WithName("GetGenreCardById")
            .Produces<GenreCardDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get GenreCard by Id")
            .WithDescription("Get GenreCard by Id")
            .WithTags("GenreCards");
    }
}
