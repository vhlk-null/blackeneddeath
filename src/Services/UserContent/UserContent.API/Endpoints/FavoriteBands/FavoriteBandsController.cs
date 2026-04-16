namespace UserContent.API.Endpoints.FavoriteBands;

public record AddBandToFavoriteRequest(Guid BandId, Guid UserId);
public record AddBandToFavoriteResponse(Guid UserId);
public record DeleteFavoriteBandResponse(bool IsSuccess);

[ApiController]
[Route("favoriteBands")]
[Tags("Favorite Bands")]
[Authorize]
public class FavoriteBandsController(IUserContentService service) : ControllerBase
{
    [HttpGet("{userId:guid}")]
    [ProducesResponseType(typeof(PaginatedResult<BandCardDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetFavoriteBands(Guid userId, CancellationToken ct, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
    {
        PaginatedResult<BandCardDto> result = await service.GetFavoriteBandsAsync(userId, pageIndex, pageSize, ct);
        return Ok(result);
    }

    [HttpGet("check")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<IActionResult> IsBandFavorite([FromQuery] Guid userId, [FromQuery] Guid bandId, CancellationToken ct)
    {
        bool isFavorite = await service.IsBandFavoriteAsync(userId, bandId, ct);
        return Ok(isFavorite);
    }

    [HttpPost]
    [ProducesResponseType(typeof(AddBandToFavoriteResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddBandToFavorite(AddBandToFavoriteRequest request, CancellationToken ct)
    {
        Guid userId = await service.AddFavoriteBandAsync(request.UserId, request.BandId, ct);
        return Created($"/favoriteBands/{userId}", new AddBandToFavoriteResponse(userId));
    }

    [HttpDelete]
    [ProducesResponseType(typeof(DeleteFavoriteBandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteFavoriteBand([FromQuery] Guid userId, [FromQuery] Guid bandId, CancellationToken ct)
    {
        await service.DeleteFavoriteBandAsync(userId, bandId, ct);
        return Ok(new DeleteFavoriteBandResponse(true));
    }
}
