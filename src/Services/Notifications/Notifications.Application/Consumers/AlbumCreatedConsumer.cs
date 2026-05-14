using System.Text.Json;

namespace Notifications.Application.Consumers;

public class AlbumCreatedConsumer(IRepository<NotificationsContext> repo, SseChannelService sseService) : IConsumer<AlbumCreatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<AlbumCreatedIntegrationEvent> context)
    {
        List<string> bandIds = context.Message.Bands.Select(b => b.Id.ToString()).ToList();

        List<Subscription> subscriptions = await repo.FilterAsync<Subscription>(
            s => s.ResourceType == ResourceTypes.Band && bandIds.Contains(s.ResourceId),
            cancellationToken: context.CancellationToken);

        if (subscriptions.Count == 0) return;

        IEnumerable<Notification> notifications = subscriptions
            .GroupBy(s => s.UserId)
            .Select(g => Notification.Create(
                g.Key,
                "New Album Released",
                $"A new album \"{context.Message.Title}\" has been released.",
                "album_created",
                context.Message.AlbumId.ToString()));

        await repo.AddRangeAsync(notifications, context.CancellationToken);
        await repo.SaveChangesAsync(context.CancellationToken);

        foreach (var notification in notifications)
        {
            await sseService.PublishAsync(notification.UserId, JsonSerializer.Serialize(
                new NotificationDto(
                    notification.Id,
                    notification.UserId,
                    notification.Title,
                    notification.Message,
                    notification.Type,
                    notification.ResourceId,
                    notification.IsRead,
                    notification.CreatedAt)));
        }
    }
}
