namespace Notifications.Application.Exceptions;

public class NotificationNotFoundException(Guid id)
    : NotFoundException($"Notification with id '{id}' was not found.");
