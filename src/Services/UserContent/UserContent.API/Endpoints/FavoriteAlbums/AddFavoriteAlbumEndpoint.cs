namespace UserContent.API.Endpoints.FavoriteAlbums;

public record AddAlbumToFavoriteRequest(Guid AlbumId, Guid UserId);
public record AddAlbumToFavoriteResponse(Guid UserId);

[ApiController]
[Route("favoriteAlbums")]
public class AddFavoriteAlbumController(IUserContentService service) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(AddAlbumToFavoriteResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddAlbumToFavorite(AddAlbumToFavoriteRequest request, CancellationToken ct)
    {
        var userId = await service.AddFavoriteAlbumAsync(request.UserId, request.AlbumId, ct);
        return Created($"/favoriteAlbums/{userId}", new AddAlbumToFavoriteResponse(userId));
    }
}
