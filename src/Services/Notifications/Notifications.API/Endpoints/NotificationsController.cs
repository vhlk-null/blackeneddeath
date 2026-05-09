using System.Threading.Channels;
using BuildingBlocks.Messaging.SSE;

namespace Notifications.API.Endpoints;

[ApiController]
[Route("")]
[Authorize]
[ServiceFilter(typeof(Filters.ExtractUserIdFilter))]
public class NotificationsController(INotificationService notificationService, SseChannelService sseService) : ControllerBase
{
    private string UserId => HttpContext.Items["UserId"] as string
        ?? throw new UnauthorizedAccessException("User id not found in token.");

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<NotificationDto>>> GetNotifications(CancellationToken cancellationToken)
    {
        IReadOnlyList<NotificationDto> notifications = await notificationService.GetUserNotificationsAsync(UserId, cancellationToken);
        return Ok(notifications);
    }

    [HttpPatch("{id:guid}/read")]
    public async Task<IActionResult> MarkAsRead(Guid id, CancellationToken cancellationToken)
    {
        await notificationService.MarkAsReadAsync(id, UserId, cancellationToken);
        return NoContent();
    }

    [HttpPatch("read-all")]
    public async Task<IActionResult> MarkAllAsRead(CancellationToken cancellationToken)
    {
        await notificationService.MarkAllAsReadAsync(UserId, cancellationToken);
        return NoContent();
    }

    [HttpGet("stream")]
    public async Task StreamNotifications(CancellationToken cancellationToken)
    {
        Response.Headers.ContentType = "text/event-stream";
        Response.Headers.CacheControl = "no-cache";
        Response.Headers.Connection = "keep-alive";
        Response.Headers.Append("X-Accel-Buffering", "no");

        ChannelReader<string> reader = sseService.Subscribe(UserId);
        try
        {
            await foreach (var message in reader.ReadAllAsync(cancellationToken))
            {
                await Response.WriteAsync($"data: {message}\n\n", cancellationToken);
                await Response.Body.FlushAsync(cancellationToken);
            }
        }
        finally
        {
            sseService.Unsubscribe(UserId, reader);
        }
    }
}
