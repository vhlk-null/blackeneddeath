using Library.Application.Services.Genres.Queries.GetGenreById;

namespace Library.API.Endpoints.Genres;

public record GetGenreByIdResponse(GenreDto Genre);

public class GetGenreById : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/genres/{id:guid}", async (Guid id, ISender sender) =>
            {
                var query = new GetGenreByIdQuery(id);
                var result = await sender.Send(query);
                var response = result.Adapt<GetGenreByIdResponse>();
                return Results.Ok(response);
            })
            .WithName("GetGenreById")
            .Produces<GetGenreByIdResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Genre By Id")
            .WithDescription("Get Genre By Id")
            .WithTags("Genres");
    }
}
