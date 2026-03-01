namespace UserContent.API.Endpoints.FavoriteAlbums;

public record DeleteFavoriteAlbumResponse(bool IsSuccess);

[ApiController]
[Route("favoriteAlbums")]
public class DeleteFavoriteAlbumController(IUserContentService service) : ControllerBase
{
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
