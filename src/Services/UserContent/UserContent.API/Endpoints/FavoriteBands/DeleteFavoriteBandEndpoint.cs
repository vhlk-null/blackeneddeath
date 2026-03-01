namespace UserContent.API.Endpoints.FavoriteBands;

public record DeleteFavoriteBandResponse(bool IsSuccess);

[ApiController]
[Route("favoriteBands")]
public class DeleteFavoriteBandController(IUserContentService service) : ControllerBase
{
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
