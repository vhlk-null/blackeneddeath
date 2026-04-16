using Microsoft.AspNetCore.Authorization;

namespace UserContent.API.Endpoints.FavoriteAlbums;

public record AddAlbumToFavoriteRequest(Guid AlbumId, Guid UserId);
public record AddAlbumToFavoriteResponse(Guid UserId);
public record DeleteFavoriteAlbumResponse(bool IsSuccess);

[ApiController]
[Route("favoriteAlbums")]
[Tags("Favorite Albums")]
[Authorize]
public class FavoriteAlbumsController(IUserContentService service) : ControllerBase
{
    [HttpGet("{userId:guid}")]
    [ProducesResponseType(typeof(PaginatedResult<AlbumCardDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetFavoriteAlbums(Guid userId, CancellationToken ct, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
    {
        PaginatedResult<AlbumCardDto> result = await service.GetFavoriteAlbumsAsync(userId, pageIndex, pageSize, ct);
        return Ok(result);
    }

    [HttpGet("check")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<IActionResult> IsAlbumFavorite([FromQuery] Guid userId, [FromQuery] Guid albumId, CancellationToken ct)
    {
        bool isFavorite = await service.IsAlbumFavoriteAsync(userId, albumId, ct);
        return Ok(isFavorite);
    }

    [HttpPost]
    [ProducesResponseType(typeof(AddAlbumToFavoriteResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddAlbumToFavorite(AddAlbumToFavoriteRequest request, CancellationToken ct)
    {
        Guid userId = await service.AddFavoriteAlbumAsync(request.UserId, request.AlbumId, ct);
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
