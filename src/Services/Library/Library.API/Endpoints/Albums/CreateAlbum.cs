using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Endpoints.Albums;

public record CreateAlbumRequest
{
    public string Album { get; init; } = string.Empty;
    public IFormFile? CoverImage { get; init; }
}
public record CreateAlbumResponse(Guid Id);

public class CreateAlbum : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/albums",
                async ([FromForm] CreateAlbumRequest request, ISender sender) =>
                {
                    var albumDto = JsonSerializer.Deserialize<CreateAlbumDto>(request.Album,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true,
                            Converters = { new JsonStringEnumConverter() }
                        });

                    if (albumDto is null)
                        return Results.Problem("Could not deserialize album data.", instance: "/albums", statusCode: StatusCodes.Status400BadRequest);

                    var command = new CreateAlbumCommand(
                        albumDto,
                        request.CoverImage?.OpenReadStream(),
                        request.CoverImage?.ContentType,
                        request.CoverImage?.FileName);
                    var result = await sender.Send(command);
                    return Results.Created($"/albums/{result.Id}", result.Adapt<CreateAlbumResponse>());
                })
            .WithName("CreatedAlbum")
            .Accepts<CreateAlbumRequest>("multipart/form-data")
            .Produces<CreateAlbumResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Album")
            .WithDescription("Create Album")
            .WithTags("Albums")
            .DisableAntiforgery();
    }
}
