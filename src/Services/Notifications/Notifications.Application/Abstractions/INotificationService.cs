namespace Notifications.Application.Abstractions;

public interface INotificationService
{
    Task<IReadOnlyList<NotificationDto>> GetUserNotificationsAsync(string userId, CancellationToken cancellationToken = default);
    Task MarkAsReadAsync(Guid notificationId, string userId, CancellationToken cancellationToken = default);
    Task MarkAllAsReadAsync(string userId, CancellationToken cancellationToken = default);
    Task<SubscriptionDto> SubscribeAsync(string userId, string resourceType, string resourceId, string resourceName, string resourceSlug, CancellationToken cancellationToken = default);
    Task UnsubscribeAsync(string userId, string resourceType, string resourceId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SubscriptionDto>> GetSubscriptionsAsync(string userId, CancellationToken cancellationToken = default);
    Task<bool> IsSubscribedAsync(string userId, string resourceType, string resourceId, CancellationToken cancellationToken = default);
}
