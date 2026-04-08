namespace Library.Application.Services.Albums.EventHandlers.Domain;

public sealed class AlbumUpdatedEventHandler(ILogger<AlbumUpdatedEventHandler> logger, IPublishEndpoint publishEndpoint)
    : INotificationHandler<AlbumUpdatedEvent>
{
    public async ValueTask Handle(AlbumUpdatedEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event handled: {DomainEvent}", domainEvent.GetType().Name);

        AlbumUpdatedIntegrationEvent integrationEvent = new AlbumUpdatedIntegrationEvent
        {
            AlbumId = domainEvent.Album.Id.Value,
            Title = domainEvent.Album.Title,
            CoverUrl = domainEvent.Album.CoverUrl,
            ReleaseDate = domainEvent.Album.AlbumRelease.ReleaseYear
        };

        await publishEndpoint.Publish(integrationEvent, cancellationToken);
    }
}
