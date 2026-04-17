using Microsoft.AspNetCore.Authorization;

namespace UserContent.API.Endpoints.AlbumReviews;

[ApiController]
[Route("album-reviews")]
[Tags("Album Reviews")]
[Authorize]
public class AlbumReviewsController(IUserContentService service) : ControllerBase
{
    [HttpGet("{albumId:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PaginatedResult<ReviewDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAlbumReviews(Guid albumId, CancellationToken ct, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 20, [FromQuery] ReviewOrderBy orderBy = ReviewOrderBy.Newest)
    {
        PaginatedResult<ReviewDto> result = await service.GetAlbumReviewsAsync(albumId, pageIndex, pageSize, orderBy, ct);
        return Ok(result);
    }

    [HttpGet("{albumId:guid}/count")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAlbumReviewCount(Guid albumId, CancellationToken ct)
    {
        int count = await service.GetAlbumReviewCountAsync(albumId, ct);
        return Ok(count);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ReviewDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAlbumReview(CreateAlbumReviewRequest request, CancellationToken ct)
    {
        ReviewDto review = await service.CreateAlbumReviewAsync(request, ct);
        return CreatedAtAction(nameof(GetAlbumReviews), new { albumId = request.AlbumId }, review);
    }

    [HttpPut("{reviewId:guid}")]
    [ProducesResponseType(typeof(ReviewDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAlbumReview(Guid reviewId, UpdateReviewRequest request, CancellationToken ct)
    {
        ReviewDto review = await service.UpdateAlbumReviewAsync(reviewId, request, ct);
        return Ok(review);
    }

    [HttpDelete("{reviewId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAlbumReview(Guid reviewId, CancellationToken ct)
    {
        await service.DeleteAlbumReviewAsync(reviewId, ct);
        return NoContent();
    }
}
