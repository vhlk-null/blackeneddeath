using System.Text.Json;

namespace Library.API.Endpoints.Albums;

public record CreateAlbumRequest(AlbumDto Album, IFormFile? CoverImage)
{
    public static async ValueTask<CreateAlbumRequest> BindAsync(HttpContext ctx)
    {
        var form = await ctx.Request.ReadFormAsync();
        var album = JsonSerializer.Deserialize<AlbumDto>(form["album"].ToString(), new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
        return new CreateAlbumRequest(album, form.Files["coverImage"]);
    }
}

public record CreateAlbumResponse(Guid Id);

public class CreateAlbum : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/albums",
                async (CreateAlbumRequest request, ISender sender) =>
                {
                    var command = new CreateAlbumCommand(
                        request.Album,
                        request.CoverImage?.OpenReadStream(),
                        request.CoverImage?.ContentType,
                        request.CoverImage?.FileName);
                    var result = await sender.Send(command);
                    return Results.Created($"/albums/{result.Id}", result.Adapt<CreateAlbumResponse>());
                })
            .WithName("CreatedAlbum")
            .Produces<CreateAlbumResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Album")
            .WithDescription("Create Album")
            .WithTags("Albums")
            .DisableAntiforgery();
    }
}
