namespace UserContent.API.Endpoints.TopRated;

[ApiController]
[Route("top-rated")]
[Tags("Top Rated")]
public class TopRatedController(IUserContentService service) : ControllerBase
{
    [HttpGet("albums")]
    [ProducesResponseType(typeof(PaginatedResult<AlbumCardDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTopRatedAlbums(
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        PaginatedResult<AlbumCardDto> result = await service.GetTopRatedAlbumsAsync(
            new PaginationRequest(pageIndex, pageSize), ct);
        return Ok(result);
    }

    [HttpGet("bands")]
    [ProducesResponseType(typeof(PaginatedResult<BandCardDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTopRatedBands(
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        PaginatedResult<BandCardDto> result = await service.GetTopRatedBandsAsync(
            new PaginationRequest(pageIndex, pageSize), ct);
        return Ok(result);
    }
}
