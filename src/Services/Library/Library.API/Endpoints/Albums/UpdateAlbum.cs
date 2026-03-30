using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Endpoints.Albums;

public record UpdateAlbumRequest
{
    public string Album { get; init; } = string.Empty;
    public IFormFile? CoverImage { get; init; }
}

public record UpdateAlbumResponse(bool IsSuccess);

public class UpdateAlbum : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/albums", async ([FromForm] UpdateAlbumRequest request, ISender sender) =>
            {
                var albumDto = JsonSerializer.Deserialize<UpdateAlbumDto>(request.Album,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        Converters = { new JsonStringEnumConverter() }
                    });

                if (albumDto is null)
                    return Results.Problem("Could not deserialize album data.", instance: "/albums", statusCode: StatusCodes.Status400BadRequest);

                var command = new UpdateAlbumCommand(
                    albumDto,
                    request.CoverImage?.OpenReadStream(),
                    request.CoverImage?.ContentType,
                    request.CoverImage?.FileName);

                var result = await sender.Send(command);
                return Results.Ok(result.Adapt<UpdateAlbumResponse>());
            })
            .WithName("UpdateAlbum")
            .Accepts<UpdateAlbumRequest>("multipart/form-data")
            .Produces<UpdateAlbumResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Update Album")
            .WithDescription("Update Album")
            .WithTags("Albums")
            .DisableAntiforgery();
    }
}
