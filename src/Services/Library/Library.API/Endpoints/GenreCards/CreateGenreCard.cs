namespace Library.API.Endpoints.GenreCards;

public record CreateGenreCardRequest
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public int OrderNumber { get; init; }
    public IFormFile? CoverImage { get; init; }
}

public record CreateGenreCardResponse(Guid Id);

public class CreateGenreCard : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/genre-cards",
                async ([FromForm] CreateGenreCardRequest request, ISender sender) =>
                {
                    Stream? coverImageStream = null;
                    if (request.CoverImage is not null)
                    {
                        coverImageStream = new MemoryStream();
                        await request.CoverImage.CopyToAsync(coverImageStream);
                        coverImageStream.Position = 0;
                    }

                    var command = new CreateGenreCardCommand(
                        request.Name,
                        request.Description,
                        request.OrderNumber,
                        coverImageStream,
                        request.CoverImage?.ContentType,
                        request.CoverImage?.FileName);

                    var result = await sender.Send(command);

                    return Results.Created($"/genre-cards/{result.Id}", result.Adapt<CreateGenreCardResponse>());
                })
            .WithName("CreateGenreCard")
            .Accepts<CreateGenreCardRequest>("multipart/form-data")
            .Produces<CreateGenreCardResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create GenreCard")
            .WithDescription("Create GenreCard")
            .WithTags("GenreCards")
            .DisableAntiforgery();
    }
}
