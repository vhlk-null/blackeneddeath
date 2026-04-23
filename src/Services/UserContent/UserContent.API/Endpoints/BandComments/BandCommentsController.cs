namespace UserContent.API.Endpoints.BandComments;

[ApiController]
[Route("band-comments")]
[Tags("Band Comments")]
[Authorize]
public class BandCommentsController(IUserContentService service) : ControllerBase
{
    [HttpGet("{bandId:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PaginatedResult<CommentDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBandComments(Guid bandId, CancellationToken ct, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 20)
    {
        PaginatedResult<CommentDto> result = await service.GetBandCommentsAsync(bandId, pageIndex, pageSize, ct);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CommentDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateBandComment(CreateBandCommentRequest request, CancellationToken ct)
    {
        CommentDto comment = await service.CreateBandCommentAsync(request, ct);
        return CreatedAtAction(nameof(GetBandComments), new { bandId = request.BandId }, comment);
    }

    [HttpPut("{commentId:guid}")]
    [ProducesResponseType(typeof(CommentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateBandComment(Guid commentId, UpdateCommentRequest request, CancellationToken ct)
    {
        CommentDto comment = await service.UpdateBandCommentAsync(commentId, request, ct);
        return Ok(comment);
    }

    [HttpDelete("{commentId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteBandComment(Guid commentId, CancellationToken ct)
    {
        await service.DeleteBandCommentAsync(commentId, ct);
        return NoContent();
    }
}
