namespace Library.Application.Services.Albums.EventHandlers.Domain;

public sealed class AlbumCreatedEventHandler(
    ILogger<AlbumCreatedEventHandler> logger,
    IPublishEndpoint publishEndpoint)
    : INotificationHandler<AlbumCreatedEvent>
{
    public async ValueTask Handle(AlbumCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event handled: {DomainEvent}", domainEvent.GetType().Name);

        // Feature flag: FeatureFlags.AlbumFulfillment
        // if (await featureManager.IsEnabledAsync(FeatureFlags.AlbumFulfillment))

        var integrationEvent = new AlbumCreatedIntegrationEvent
        {
            AlbumId = domainEvent.Album.Id.Value,
            Title = domainEvent.Album.Title,
            CoverUrl = domainEvent.Album.CoverUrl,
            ReleaseYear = domainEvent.Album.AlbumRelease.ReleaseYear,
            Format = (int)domainEvent.Album.AlbumRelease.Format,
            Type = (int)domainEvent.Album.Type,
            Label = null
        };

        await publishEndpoint.Publish(integrationEvent, cancellationToken);
    }
}
