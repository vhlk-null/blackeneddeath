namespace Library.Application.Services.Albums.EventHandlers;

public class AlbumCreatedEventHandler(ILogger<AlbumCreatedEventHandler> logger, IPublishEndpoint publishEndpoint) : INotificationHandler<AlbumCreatedEvent>
{
    public async ValueTask Handle(AlbumCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event handled: {DomainEvent}", notification.GetType().Name);

        var integrationEvent = notification.Album.Adapt<AlbumCreatedIntegrationEvent>();

        await publishEndpoint.Publish(integrationEvent, cancellationToken);
    }
}