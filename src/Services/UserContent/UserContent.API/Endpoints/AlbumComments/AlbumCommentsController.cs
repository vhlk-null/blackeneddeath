namespace UserContent.API.Endpoints.AlbumComments;

[ApiController]
[Route("album-comments")]
[Tags("Album Comments")]
[Authorize]
public class AlbumCommentsController(IUserContentService service) : ControllerBase
{
    [HttpGet("{albumId:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PaginatedResult<CommentDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAlbumComments(Guid albumId, CancellationToken ct, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 20)
    {
        PaginatedResult<CommentDto> result = await service.GetAlbumCommentsAsync(albumId, pageIndex, pageSize, ct);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CommentDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAlbumComment(CreateAlbumCommentRequest request, CancellationToken ct)
    {
        CommentDto comment = await service.CreateAlbumCommentAsync(request, ct);
        return CreatedAtAction(nameof(GetAlbumComments), new { albumId = request.AlbumId }, comment);
    }

    [HttpPut("{commentId:guid}")]
    [ProducesResponseType(typeof(CommentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAlbumComment(Guid commentId, UpdateCommentRequest request, CancellationToken ct)
    {
        CommentDto comment = await service.UpdateAlbumCommentAsync(commentId, request, ct);
        return Ok(comment);
    }

    [HttpDelete("{commentId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAlbumComment(Guid commentId, CancellationToken ct)
    {
        await service.DeleteAlbumCommentAsync(commentId, ct);
        return NoContent();
    }
}
