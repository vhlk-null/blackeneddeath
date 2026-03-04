namespace Library.Application.Services.Albums.EventHandlers.Integration;

public sealed class AlbumCreatedEventHandler(ILogger<AlbumCreatedEventHandler> logger, IPublishEndpoint publishEndpoint)
    : AlbumIntegrationEventHandler<AlbumCreatedEvent, AlbumCreatedIntegrationEvent>(logger, publishEndpoint);
