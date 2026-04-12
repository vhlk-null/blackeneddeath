using Microsoft.AspNetCore.Authorization;

namespace UserContent.API.Endpoints.BandRatings;

public record RateBandRequest(Guid UserId, Guid BandId, int Rating);
public record GetBandRatingResponse(Guid BandId, int? Rating);

[ApiController]
[Route("bandRatings")]
[Tags("Band Ratings")]
[Authorize]
public class BandRatingsController(IUserContentService service) : ControllerBase
{
    [HttpGet("{bandId:guid}")]
    [ProducesResponseType(typeof(GetBandRatingResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBandRating(Guid bandId, [FromQuery] Guid userId, CancellationToken ct)
    {
        int? rating = await service.GetBandRatingAsync(userId, bandId, ct);
        return Ok(new GetBandRatingResponse(bandId, rating));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RateBand(RateBandRequest request, CancellationToken ct)
    {
        await service.RateBandAsync(request.UserId, request.BandId, request.Rating, ct);
        return NoContent();
    }
}
