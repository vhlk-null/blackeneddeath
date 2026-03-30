using Library.Application.Services.Albums.Commands.UpdateAlbumCover;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Endpoints.Albums;

public record UpdateAlbumCoverRequest
{
    public IFormFile CoverImage { get; init; } = null!;
}

public record UpdateAlbumCoverResponse(bool IsSuccess);

public class UpdateAlbumCover : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/albums/{id:guid}/cover",
                async (Guid id, [FromForm] UpdateAlbumCoverRequest request, ISender sender) =>
                {
                    var command = new UpdateAlbumCoverCommand(
                        id,
                        request.CoverImage.OpenReadStream(),
                        request.CoverImage.ContentType,
                        request.CoverImage.FileName);

                    var result = await sender.Send(command);

                    return Results.Ok(result.Adapt<UpdateAlbumCoverResponse>());
                })
            .WithName("UpdateAlbumCover")
            .Accepts<UpdateAlbumCoverRequest>("multipart/form-data")
            .Produces<UpdateAlbumCoverResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update Album Cover")
            .WithDescription("Upload or replace the cover image for an album")
            .WithTags("Albums")
            .DisableAntiforgery();
    }
}
