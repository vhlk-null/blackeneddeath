namespace BuildingBlocks.Messaging.Events.Albums;

public class AlbumRemovedIntegrationEvent : IntegrationEvent
{
    public Guid AlbumId { get; init; }
    public string Title { get; set; } = null!;
}