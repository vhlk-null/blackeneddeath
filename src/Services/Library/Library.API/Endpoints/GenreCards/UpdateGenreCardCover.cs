namespace Library.API.Endpoints.GenreCards;

public record UpdateGenreCardCoverRequest
{
    public IFormFile CoverImage { get; init; } = null!;
}

public record UpdateGenreCardCoverResponse(bool IsSuccess);

public class UpdateGenreCardCover : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/genre-cards/{id:guid}/cover",
                async (Guid id, [FromForm] UpdateGenreCardCoverRequest request, ISender sender) =>
                {
                    var coverImageStream = new MemoryStream();
                    await request.CoverImage.CopyToAsync(coverImageStream);
                    coverImageStream.Position = 0;

                    var command = new UpdateGenreCardCoverCommand(
                        id,
                        coverImageStream,
                        request.CoverImage.ContentType,
                        request.CoverImage.FileName);

                    var result = await sender.Send(command);

                    return Results.Ok(result.Adapt<UpdateGenreCardCoverResponse>());
                })
            .WithName("UpdateGenreCardCover")
            .Accepts<UpdateGenreCardCoverRequest>("multipart/form-data")
            .Produces<UpdateGenreCardCoverResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update GenreCard Cover")
            .WithDescription("Upload or replace the cover image for a genre card")
            .WithTags("GenreCards")
            .DisableAntiforgery();
    }
}
