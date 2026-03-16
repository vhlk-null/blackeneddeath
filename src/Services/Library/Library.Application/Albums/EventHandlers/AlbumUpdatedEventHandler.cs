namespace Library.Application.Albums.EventHandlers;

public class AlbumUpdatedEventHandler(ILogger<AlbumUpdatedEventHandler> logger) : INotificationHandler<AlbumUpdatedEvent>
{
    public async ValueTask Handle(AlbumUpdatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event handled: {DomainEvent}", notification.GetType().Name);
    }
}