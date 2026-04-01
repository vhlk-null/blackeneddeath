namespace Library.API.Endpoints.Genres;

public record GetGenresResult(IReadOnlyList<GenreDto> Genres);

public record GenreDto(Guid Id, string Name, Guid? ParentGenreId);

public class GetGenres : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/genres", async (ISender sender) =>
            {
                var result = await sender.Send(new GetGenresQuery());
                var response = result.Adapt<GetGenresResult>();
                return Results.Ok(response);
            })
            .WithName("GetGenres")
            .Produces<GetGenresResult>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Genres")
            .WithDescription("Get Genres")
            .WithTags("Genres");
    }
}
