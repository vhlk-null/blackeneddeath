namespace UserContent.API.Endpoints.FavoriteBands;

public record AddBandToFavoriteRequest(Guid BandId, Guid UserId);
public record AddBandToFavoriteResponse(Guid UserId);
public record DeleteFavoriteBandResponse(bool IsSuccess);

[ApiController]
[Route("favoriteBands")]
public class FavoriteBandsController(IUserContentService service) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(AddBandToFavoriteResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddBandToFavorite(AddBandToFavoriteRequest request, CancellationToken ct)
    {
        var userId = await service.AddFavoriteBandAsync(request.UserId, request.BandId, ct);
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
