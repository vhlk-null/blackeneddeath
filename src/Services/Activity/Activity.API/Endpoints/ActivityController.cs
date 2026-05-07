namespace Activity.API.Endpoints;

[ApiController]
[Route("")]
[Authorize]
[ServiceFilter(typeof(Filters.ExtractUserIdFilter))]
public class ActivityController(IActivityService activityService) : ControllerBase
{
    private Guid UserId => Guid.Parse(
        HttpContext.Items["UserId"] as string
        ?? throw new UnauthorizedAccessException("User id not found in token."));

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<UserActivityDto>>> GetActivity(CancellationToken cancellationToken)
    {
        var result = await activityService.GetUserActivityAsync(UserId, cancellationToken);
        return Ok(result);
    }

    // TODO: remove — for local testing only
    [HttpPost("test")]
    [AllowAnonymous]
    public async Task<IActionResult> RecordTest(
        [FromBody] RecordTestRequest request,
        CancellationToken cancellationToken)
    {
        await activityService.RecordAsync(request.UserId, request.Type, request.Payload, cancellationToken);
        return Ok();
    }
}

public record RecordTestRequest(Guid UserId, string Type, Dictionary<string, string> Payload);
