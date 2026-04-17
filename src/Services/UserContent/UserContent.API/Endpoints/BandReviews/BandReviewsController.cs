using Microsoft.AspNetCore.Authorization;

namespace UserContent.API.Endpoints.BandReviews;

[ApiController]
[Route("band-reviews")]
[Tags("Band Reviews")]
[Authorize]
public class BandReviewsController(IUserContentService service) : ControllerBase
{
    [HttpGet("{bandId:guid}")]
    [ProducesResponseType(typeof(PaginatedResult<ReviewDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBandReviews(Guid bandId, CancellationToken ct, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 20)
    {
        PaginatedResult<ReviewDto> result = await service.GetBandReviewsAsync(bandId, pageIndex, pageSize, ct);
        return Ok(result);
    }

    [HttpGet("{bandId:guid}/count")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBandReviewCount(Guid bandId, CancellationToken ct)
    {
        int count = await service.GetBandReviewCountAsync(bandId, ct);
        return Ok(count);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ReviewDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateBandReview(CreateBandReviewRequest request, CancellationToken ct)
    {
        ReviewDto review = await service.CreateBandReviewAsync(request, ct);
        return CreatedAtAction(nameof(GetBandReviews), new { bandId = request.BandId }, review);
    }

    [HttpDelete("{reviewId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteBandReview(Guid reviewId, CancellationToken ct)
    {
        await service.DeleteBandReviewAsync(reviewId, ct);
        return NoContent();
    }
}
