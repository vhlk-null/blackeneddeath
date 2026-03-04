namespace Library.Application.Services.Albums.EventHandlers.Integration;

public sealed class AlbumRemovedEventHandler(ILogger<AlbumRemovedEventHandler> logger, IPublishEndpoint publishEndpoint)
    : AlbumIntegrationEventHandler<AlbumRemovedEvent, AlbumRemovedIntegrationEvent>(logger, publishEndpoint);
