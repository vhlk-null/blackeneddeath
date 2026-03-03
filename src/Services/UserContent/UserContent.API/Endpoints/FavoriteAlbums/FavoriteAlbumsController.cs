namespace UserContent.API.Endpoints.FavoriteAlbums;

public record AddAlbumToFavoriteRequest(Guid AlbumId, Guid UserId);
public record AddAlbumToFavoriteResponse(Guid UserId);
public record DeleteFavoriteAlbumResponse(bool IsSuccess);

[ApiController]
[Route("favoriteAlbums")]
public class FavoriteAlbumsController(IUserContentService service) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(AddAlbumToFavoriteResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddAlbumToFavorite(AddAlbumToFavoriteRequest request, CancellationToken ct)
    {
        var userId = await service.AddFavoriteAlbumAsync(request.UserId, request.AlbumId, ct);
        return Created($"/favoriteAlbums/{userId}", new AddAlbumToFavoriteResponse(userId));
    }

    [HttpDelete]
    [ProducesResponseType(typeof(DeleteFavoriteAlbumResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteFavoriteAlbum([FromQuery] Guid userId, [FromQuery] Guid albumId, CancellationToken ct)
    {
        await service.DeleteFavoriteAlbumAsync(userId, albumId, ct);
        return Ok(new DeleteFavoriteAlbumResponse(true));
    }
}
