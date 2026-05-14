namespace Notifications.Application.Services;

public class NotificationService(IRepository<NotificationsContext> repo) : INotificationService
{
    public async Task<IReadOnlyList<NotificationDto>> GetUserNotificationsAsync(string userId, CancellationToken cancellationToken = default)
    {
        List<Notification> notifications = await repo.FilterAsync<Notification>(
            n => n.UserId == userId && !n.IsRead,
            cancellationToken: cancellationToken);

        return notifications
            .OrderByDescending(n => n.CreatedAt)
            .Select(ToDto)
            .ToList();
    }

    public async Task MarkAsReadAsync(Guid notificationId, string userId, CancellationToken cancellationToken = default)
    {
        Notification notification = await repo.GetByAsync<Notification>(
            n => n.Id == notificationId && n.UserId == userId,
            cancellationToken: cancellationToken)
            ?? throw new NotificationNotFoundException(notificationId);

        notification.MarkAsRead();
        await repo.SaveChangesAsync(cancellationToken);
    }

    public async Task MarkAllAsReadAsync(string userId, CancellationToken cancellationToken = default)
    {
        List<Notification> notifications = await repo.FilterAsync<Notification>(
            n => n.UserId == userId && !n.IsRead,
            cancellationToken: cancellationToken);

        foreach (Notification notification in notifications)
            notification.MarkAsRead();

        await repo.SaveChangesAsync(cancellationToken);
    }

    public async Task<SubscriptionDto> SubscribeAsync(string userId, string resourceType, string resourceId, string resourceName, string resourceSlug, CancellationToken cancellationToken = default)
    {
        Subscription? existing = await repo.GetByAsync<Subscription>(
            s => s.UserId == userId && s.ResourceType == resourceType && s.ResourceId == resourceId,
            cancellationToken: cancellationToken);

        if (existing is not null)
            return ToDto(existing);

        Subscription subscription = Subscription.Create(userId, resourceType, resourceId, resourceName, resourceSlug);
        await repo.AddAsync(subscription, cancellationToken);
        await repo.SaveChangesAsync(cancellationToken);

        return ToDto(subscription);
    }

    public async Task UnsubscribeAsync(string userId, string resourceType, string resourceId, CancellationToken cancellationToken = default)
    {
        Subscription? subscription = await repo.GetByAsync<Subscription>(
            s => s.UserId == userId && s.ResourceType == resourceType && s.ResourceId == resourceId,
            cancellationToken: cancellationToken);

        if (subscription is null) return;

        repo.Delete(subscription);
        await repo.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> IsSubscribedAsync(string userId, string resourceType, string resourceId, CancellationToken cancellationToken = default)
    {
        Subscription? subscription = await repo.GetByAsync<Subscription>(
            s => s.UserId == userId && s.ResourceType == resourceType && s.ResourceId == resourceId,
            cancellationToken: cancellationToken);

        return subscription is not null;
    }

    public async Task<IReadOnlyList<SubscriptionDto>> GetSubscriptionsAsync(string userId, CancellationToken cancellationToken = default)
    {
        List<Subscription> subscriptions = await repo.FilterAsync<Subscription>(
            s => s.UserId == userId,
            cancellationToken: cancellationToken);

        return subscriptions.Select(ToDto).ToList();
    }

    private static NotificationDto ToDto(Notification n) =>
        new(n.Id, n.UserId, n.Title, n.Message, n.Type, n.ResourceId, null, n.IsRead, n.CreatedAt);

    private static SubscriptionDto ToDto(Subscription s) =>
        new(s.Id, s.UserId, s.ResourceType, s.ResourceId, s.ResourceName, s.ResourceSlug, s.CreatedAt);
}
