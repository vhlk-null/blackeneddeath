namespace Library.Application.Services.Albums.EventHandlers;

public class AlbumRemovedEventHandler(ILogger<AlbumRemovedEventHandler> logger, IPublishEndpoint publishEndpoint) : INotificationHandler<AlbumRemovedEvent>
{
    public async ValueTask Handle(AlbumRemovedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event handled: {DomainEvent}", notification.GetType().Name);


        var integrationEvent = notification.Album.Adapt<AlbumRemovedIntegrationEvent>();

        await publishEndpoint.Publish(integrationEvent, cancellationToken);
    }
}
