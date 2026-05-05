namespace Notifications.API.Endpoints;

[ApiController]
[Route("subscriptions")]
[Authorize]
[ServiceFilter(typeof(Filters.ExtractUserIdFilter))]
public class SubscriptionsController(INotificationService notificationService) : ControllerBase
{
    private string UserId => HttpContext.Items["UserId"] as string
        ?? throw new UnauthorizedAccessException("User id not found in token.");

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<SubscriptionDto>>> GetSubscriptions(CancellationToken cancellationToken)
    {
        IReadOnlyList<SubscriptionDto> subscriptions = await notificationService.GetSubscriptionsAsync(UserId, cancellationToken);
        return Ok(subscriptions);
    }

    [HttpGet("{resourceType}/{resourceId}")]
    public async Task<ActionResult<bool>> IsSubscribed(string resourceType, string resourceId, CancellationToken cancellationToken)
    {
        bool isSubscribed = await notificationService.IsSubscribedAsync(UserId, resourceType, resourceId, cancellationToken);
        return Ok(isSubscribed);
    }

    public record SubscribeRequest(string ResourceName, string ResourceSlug);

    [HttpPost("{resourceType}/{resourceId}")]
    public async Task<ActionResult<SubscriptionDto>> Subscribe(string resourceType, string resourceId, [FromBody] SubscribeRequest request, CancellationToken cancellationToken)
    {
        SubscriptionDto subscription = await notificationService.SubscribeAsync(UserId, resourceType, resourceId, request.ResourceName, request.ResourceSlug, cancellationToken);
        return Ok(subscription);
    }

    [HttpDelete("{resourceType}/{resourceId}")]
    public async Task<IActionResult> Unsubscribe(string resourceType, string resourceId, CancellationToken cancellationToken)
    {
        await notificationService.UnsubscribeAsync(UserId, resourceType, resourceId, cancellationToken);
        return NoContent();
    }
}
