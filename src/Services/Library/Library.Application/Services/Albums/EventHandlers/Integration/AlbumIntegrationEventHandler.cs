namespace Library.Application.Services.Albums.EventHandlers.Integration;

public abstract class AlbumIntegrationEventHandler<TDomainEvent, TIntegrationEvent>(
    ILogger logger,
    IPublishEndpoint publishEndpoint)
    : INotificationHandler<TDomainEvent>
    where TDomainEvent : IAlbumDomainEvent
    where TIntegrationEvent : class
{
    public async ValueTask Handle(TDomainEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event handled: {DomainEvent}", typeof(TDomainEvent).Name);

        var integrationEvent = notification.Album.Adapt<TIntegrationEvent>();

        await publishEndpoint.Publish(integrationEvent, cancellationToken);
    }
}
