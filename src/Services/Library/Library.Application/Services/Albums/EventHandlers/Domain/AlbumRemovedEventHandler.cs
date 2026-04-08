namespace Library.Application.Services.Albums.EventHandlers.Domain;

public sealed class AlbumRemovedEventHandler(ILogger<AlbumRemovedEventHandler> logger, IPublishEndpoint publishEndpoint)
    : INotificationHandler<AlbumRemovedEvent>
{
    public async ValueTask Handle(AlbumRemovedEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event handled: {DomainEvent}", domainEvent.GetType().Name);

        AlbumRemovedIntegrationEvent integrationEvent = new AlbumRemovedIntegrationEvent
        {
            AlbumId = domainEvent.Album.Id.Value,
            Title = domainEvent.Album.Title
        };

        await publishEndpoint.Publish(integrationEvent, cancellationToken);
    }
}
