using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Notifications.Application.Consumers
{
    public class AlbumReleaseConsumer(IRepository<NotificationsContext> repo, SseChannelService sseService) : IConsumer<AlbumReleaseIntegrationEvent>
    {
        public async Task Consume(ConsumeContext<AlbumReleaseIntegrationEvent> context)
        {
            List<Subscription> subscriptions = await repo.FilterAsync<Subscription>
                (s => s.ResourceType == ResourceTypes.Album && s.ResourceId == context.Message.AlbumId.ToString(), cancellationToken: context.CancellationToken);

            if (subscriptions.Count == 0) return;

            IEnumerable<Notification> notifications = subscriptions.GroupBy(s => s.UserId)
                .Select(g => Notification.Create(
                    userId: g.Key,
                    title: $"New Album Release: {context.Message.Title}",
                    message: $"The album '{context.Message.Title}' has been released in {context.Message.ReleaseYear}.",
                    type: "album_release",
                    resourceId: context.Message.AlbumId.ToString()
                ));

            await repo.AddRangeAsync(notifications, context.CancellationToken);
            repo.DeleteRange(subscriptions);
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

}