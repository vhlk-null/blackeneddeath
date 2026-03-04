namespace Library.Application.Services.Albums.EventHandlers.Integration;

public sealed class AlbumUpdatedEventHandler(ILogger<AlbumUpdatedEventHandler> logger, IPublishEndpoint publishEndpoint)
    : AlbumIntegrationEventHandler<AlbumUpdatedEvent, AlbumUpdatedIntegrationEvent>(logger, publishEndpoint);
