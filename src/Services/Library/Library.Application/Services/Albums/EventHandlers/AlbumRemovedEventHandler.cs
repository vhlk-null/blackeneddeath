namespace Library.Application.Services.Albums.EventHandlers;

public class AlbumRemovedEventHandler(ILogger<AlbumRemovedEventHandler> logger) : INotificationHandler<AlbumRemovedEvent>
{
    public async ValueTask Handle(AlbumRemovedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event handled: {DomainEvent}", notification.GetType().Name);
    }
}
