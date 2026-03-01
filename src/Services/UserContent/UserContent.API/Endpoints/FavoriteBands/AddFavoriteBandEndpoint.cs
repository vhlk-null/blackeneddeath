namespace UserContent.API.Endpoints.FavoriteBands;

public record AddBandToFavoriteRequest(Guid BandId, Guid UserId);
public record AddBandToFavoriteResponse(Guid UserId);

[ApiController]
[Route("favoriteBands")]
public class AddFavoriteBandController(IUserContentService service) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(AddBandToFavoriteResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddBandToFavorite(AddBandToFavoriteRequest request, CancellationToken ct)
    {
        var userId = await service.AddFavoriteBandAsync(request.UserId, request.BandId, ct);
        return Created($"/favoriteBands/{userId}", new AddBandToFavoriteResponse(userId));
    }
}
