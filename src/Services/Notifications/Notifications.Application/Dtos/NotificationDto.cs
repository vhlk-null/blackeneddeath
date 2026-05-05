namespace Notifications.Application.Dtos;

public record NotificationDto(
    Guid Id,
    string UserId,
    string Title,
    string Message,
    string Type,
    string? ResourceId,
    bool IsRead,
    DateTime CreatedAt);

public record SubscriptionDto(
    Guid Id,
    string UserId,
    string ResourceType,
    string ResourceId,
    string ResourceName,
    string ResourceSlug,
    DateTime CreatedAt);
