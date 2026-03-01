namespace UserContent.API.Endpoints.UserProfile;

[ApiController]
[Route("profile")]
public class GetUserProfileController(IUserContentService service) : ControllerBase
{
    [HttpGet("{userId:guid}")]
    [ProducesResponseType(typeof(UserProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserProfile(Guid userId, CancellationToken ct)
    {
        var profile = await service.GetUserProfileAsync(userId, ct);
        return Ok(profile);
    }
}
