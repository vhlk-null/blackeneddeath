namespace UserContent.API.Endpoints.UserProfile;

[ApiController]
[Route("profile")]
[Tags("User Profile")]
[Authorize]
public class UserProfileController(IUserContentService service) : ControllerBase
{
    [HttpGet("{userId:guid}")]
    [ProducesResponseType(typeof(UserProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserProfile(Guid userId, CancellationToken ct)
    {
        UserProfileDto profile = await service.GetUserProfileAsync(userId, ct);
        return Ok(profile);
    }

    [HttpPatch("{userId:guid}")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(UserProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUserProfile(
        Guid userId,
        [FromForm] string? username,
        [FromForm] string? bio,
        IFormFile? profileImage,
        IFormFile? backgroundImage,
        CancellationToken ct)
    {
        var request = new UpdateUserProfileRequest(username, bio);

        Stream? profileStream = profileImage is not null ? profileImage.OpenReadStream() : null;
        Stream? backgroundStream = backgroundImage is not null ? backgroundImage.OpenReadStream() : null;

        UserProfileDto updated = await service.UpdateUserProfileAsync(
            userId, request,
            profileStream, profileImage?.ContentType, profileImage?.FileName,
            backgroundStream, backgroundImage?.ContentType, backgroundImage?.FileName,
            ct);

        return Ok(updated);
    }
}
