namespace Library.Application.Services.Albums.EventHandlers;

public class AlbumUpdatedEventHandler(ILogger<AlbumUpdatedEventHandler> logger, IPublishEndpoint publishEndpoint) : INotificationHandler<AlbumUpdatedEvent>
{
    public async ValueTask Handle(AlbumUpdatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event handled: {DomainEvent}", notification.GetType().Name);

        var integrationEvent = notification.Album.Adapt<AlbumUpdatedIntegrationEvent>();

        await publishEndpoint.Publish(integrationEvent, cancellationToken);
    }
}