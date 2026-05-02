namespace UserContent.API.Endpoints.Admin;

[ApiController]
[Route("admin")]
[Tags("Admin")]
[Authorize(Policy = "AdminOnly")]
public class AdminController(IUserContentService service) : ControllerBase
{
    [HttpGet("users")]
    [ProducesResponseType(typeof(PaginatedResult<AdminUserDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllUsers(
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        PaginatedResult<AdminUserDto> result = await service.GetAllUsersAsync(pageIndex, pageSize, ct);
        return Ok(result);
    }
}
