namespace Library.API.Endpoints.Genres;

public record GetGenresByParentResponse(IEnumerable<GenreDto> Genres);

public class GetGenresByParent : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/genres/parent/{parentId}", async (Guid parentId, ISender sender) =>
            {
                var query = new GetGenresByParentQuery(parentId);
                var result = await sender.Send(query);
                return Results.Ok(result.Adapt<GetGenresByParentResponse>());
            })
            .WithName("GetGenresByParent")
            .Produces<GetGenresByParentResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Genres By Parent")
            .WithDescription("Get Genres By Parent")
            .WithTags("Genres");
    }
}
