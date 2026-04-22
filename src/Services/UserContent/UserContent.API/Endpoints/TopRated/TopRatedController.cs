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
        [FromQuery] string? period = null,
        [FromQuery] string? sortDir = null,
        [FromQuery] List<AlbumType>? types = null,
        [FromQuery] int? yearFrom = null,
        [FromQuery] int? yearTo = null,
        [FromQuery] string? genre = null,
        [FromQuery] string? name = null,
        CancellationToken ct = default)
    {
        RatingPeriod ratingPeriod = Enum.TryParse<RatingPeriod>(period, ignoreCase: true, out RatingPeriod p) ? p : RatingPeriod.All;
        SortDir dir = Enum.TryParse<SortDir>(sortDir, ignoreCase: true, out SortDir sd) ? sd : SortDir.Desc;
        ISpecification<Album>? filter = AlbumFilterBuilder.Build(types ?? [], yearFrom, yearTo, genre, name);
        PaginatedResult<AlbumCardDto> result = await service.GetTopRatedAlbumsAsync(
            new PaginationRequest(pageIndex, pageSize), ratingPeriod, dir, filter, ct);
        return Ok(result);
    }

    [HttpGet("bands")]
    [ProducesResponseType(typeof(PaginatedResult<BandCardDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTopRatedBands(
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? period = null,
        [FromQuery] string? sortDir = null,
        [FromQuery] string? status = null,
        [FromQuery] int? yearFrom = null,
        [FromQuery] int? yearTo = null,
        [FromQuery] string? genre = null,
        [FromQuery] string? name = null,
        CancellationToken ct = default)
    {
        RatingPeriod ratingPeriod = Enum.TryParse<RatingPeriod>(period, ignoreCase: true, out RatingPeriod p) ? p : RatingPeriod.All;
        SortDir dir = Enum.TryParse<SortDir>(sortDir, ignoreCase: true, out SortDir sd) ? sd : SortDir.Desc;
        BandStatus? bandStatus = Enum.TryParse<BandStatus>(status, ignoreCase: true, out BandStatus bs) ? bs : null;
        ISpecification<Band>? filter = BandFilterBuilder.Build(bandStatus, yearFrom, yearTo, genre, name);
        PaginatedResult<BandCardDto> result = await service.GetTopRatedBandsAsync(
            new PaginationRequest(pageIndex, pageSize), ratingPeriod, dir, filter, ct);
        return Ok(result);
    }
}
