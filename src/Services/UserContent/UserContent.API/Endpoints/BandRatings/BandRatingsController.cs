using Microsoft.AspNetCore.Authorization;

namespace UserContent.API.Endpoints.BandRatings;

public record RateBandRequest(Guid UserId, Guid BandId, int Rating);
public record GetBandRatingResponse(Guid BandId, int? UserRating, double? AverageRating, int RatingsCount);
public record RateBandResponse(Guid BandId, int UserRating, double? AverageRating, int RatingsCount);
public record GetBandAverageRatingResponse(Guid BandId, double? AverageRating, int RatingsCount);

[ApiController]
[Route("bandRatings")]
[Tags("Band Ratings")]
[Authorize]
public class BandRatingsController(IUserContentService service) : ControllerBase
{
    [HttpGet("{bandId:guid}/average")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(GetBandAverageRatingResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBandAverageRating(Guid bandId, CancellationToken ct)
    {
        (double? averageRating, int ratingsCount) = await service.GetBandAverageRatingAsync(bandId, ct);
        return Ok(new GetBandAverageRatingResponse(bandId, averageRating, ratingsCount));
    }

    [HttpGet("{bandId:guid}")]
    [ProducesResponseType(typeof(GetBandRatingResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBandRating(Guid bandId, [FromQuery] Guid userId, CancellationToken ct)
    {
        (int? userRating, double? averageRating, int ratingsCount) = await service.GetBandRatingAsync(userId, bandId, ct);
        return Ok(new GetBandRatingResponse(bandId, userRating, averageRating, ratingsCount));
    }

    [HttpPost]
    [ProducesResponseType(typeof(RateBandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RateBand(RateBandRequest request, CancellationToken ct)
    {
        (double? averageRating, int ratingsCount) = await service.RateBandAsync(request.UserId, request.BandId, request.Rating, ct);
        return Ok(new RateBandResponse(request.BandId, request.Rating, averageRating, ratingsCount));
    }
}
