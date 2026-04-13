using Microsoft.AspNetCore.Authorization;

namespace UserContent.API.Endpoints.AlbumRatings;

public record RateAlbumRequest(Guid UserId, Guid AlbumId, int Rating);
public record GetAlbumRatingResponse(Guid AlbumId, int? UserRating, double? AverageRating, int RatingsCount);
public record RateAlbumResponse(Guid AlbumId, int UserRating, double? AverageRating, int RatingsCount);
public record GetAlbumAverageRatingResponse(Guid AlbumId, double? AverageRating, int RatingsCount);

[ApiController]
[Route("albumRatings")]
[Tags("Album Ratings")]
[Authorize]
public class AlbumRatingsController(IUserContentService service) : ControllerBase
{
    [HttpGet("{albumId:guid}/average")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(GetAlbumAverageRatingResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAlbumAverageRating(Guid albumId, CancellationToken ct)
    {
        (double? averageRating, int ratingsCount) = await service.GetAlbumAverageRatingAsync(albumId, ct);
        return Ok(new GetAlbumAverageRatingResponse(albumId, averageRating, ratingsCount));
    }

    [HttpGet("{albumId:guid}")]
    [ProducesResponseType(typeof(GetAlbumRatingResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAlbumRating(Guid albumId, [FromQuery] Guid userId, CancellationToken ct)
    {
        (int? userRating, double? averageRating, int ratingsCount) = await service.GetAlbumRatingAsync(userId, albumId, ct);
        return Ok(new GetAlbumRatingResponse(albumId, userRating, averageRating, ratingsCount));
    }

    [HttpPost]
    [ProducesResponseType(typeof(RateAlbumResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RateAlbum(RateAlbumRequest request, CancellationToken ct)
    {
        (double? averageRating, int ratingsCount) = await service.RateAlbumAsync(request.UserId, request.AlbumId, request.Rating, ct);
        return Ok(new RateAlbumResponse(request.AlbumId, request.Rating, averageRating, ratingsCount));
    }
}
