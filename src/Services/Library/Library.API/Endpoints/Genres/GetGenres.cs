using Library.Application.Genres.Queries.GetGenres;

namespace Library.API.Endpoints.Genres;

public record GetGenresResult(PaginatedResult<GenreDto> Genres);

public record GenreDto(Guid Id, string Name, Guid? ParentGenreId);

public class GetGenres : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/genres", async ([AsParameters] PaginationRequest paginationRequest, ISender sender) =>
            {
                var query = new GetGenresQuery(paginationRequest);
                var result = await sender.Send(query);
                var response = result.Adapt<GetGenresResult>();
                return Results.Ok(response);
            })
            .WithName("GetGenres")
            .Produces<GetGenresResult>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Genres")
            .WithDescription("Get Genres");
    }
}
