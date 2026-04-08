namespace Library.API.Endpoints.Genres;

public record CreateGenreRequest(string Name, Guid? ParentGenreId);

public record CreateGenreResponse(Guid Id);

public class CreateGenre : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/genres",
                async (CreateGenreRequest request, ISender sender) =>
                {
                    CreateGenreCommand command = request.Adapt<CreateGenreCommand>();

                    CreateGenreResult result = await sender.Send(command);

                    CreateGenreResponse response = result.Adapt<CreateGenreResponse>();

                    return Results.Created($"/genres/{response.Id}", response);
                })
            .WithName("CreateGenre")
            .Produces<CreateGenreResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Genre")
            .WithDescription("Create Genre")
            .WithTags("Genres");
    }
}
