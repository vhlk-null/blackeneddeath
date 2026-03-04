namespace Library.Application.Services.Albums.EventHandlers;

public class AlbumCreatedEventHandler(ILogger<AlbumCreatedEventHandler> logger) : INotificationHandler<AlbumCreatedEvent>
{
    public async ValueTask Handle(AlbumCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event handled: {DomainEvent}", notification.GetType().Name);
    }
}