namespace Library.API.Endpoints.GenreCards;

public record UpdateGenreCardRequest
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public int OrderNumber { get; init; }
    public List<Guid> GenreIds { get; init; } = [];
    public List<Guid> TagIds { get; init; } = [];
    public IFormFile? CoverImage { get; init; }
}

public record UpdateGenreCardResponse(bool IsSuccess);

public class UpdateGenreCard : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/genre-cards/{id:guid}",
                async (Guid id, [FromForm] UpdateGenreCardRequest request, ISender sender) =>
                {
                    Stream? coverImageStream = null;
                    if (request.CoverImage is not null)
                    {
                        coverImageStream = new MemoryStream();
                        await request.CoverImage.CopyToAsync(coverImageStream);
                        coverImageStream.Position = 0;
                    }

                    UpdateGenreCardCommand command = new UpdateGenreCardCommand(
                        id,
                        request.Name,
                        request.Description,
                        request.OrderNumber,
                        request.GenreIds,
                        request.TagIds,
                        coverImageStream,
                        request.CoverImage?.ContentType,
                        request.CoverImage?.FileName);

                    UpdateGenreCardResult result = await sender.Send(command);

                    return Results.Ok(result.Adapt<UpdateGenreCardResponse>());
                })
            .WithName("UpdateGenreCard")
            .Accepts<UpdateGenreCardRequest>("multipart/form-data")
            .Produces<UpdateGenreCardResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update GenreCard")
            .WithDescription("Update GenreCard")
            .WithTags("GenreCards")
            .DisableAntiforgery()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .RequireAuthorization("AdminOnly");
    }
}
