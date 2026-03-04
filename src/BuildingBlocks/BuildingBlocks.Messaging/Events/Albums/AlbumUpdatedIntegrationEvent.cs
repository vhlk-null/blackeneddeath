namespace BuildingBlocks.Messaging.Events.Albums;

internal class AlbumUpdatedIntegrationEvent : IntegrationEvent
{
    public Guid AlbumId { get; set; }
    public string Title { get; set; } = null!;
    public string? CoverUrl { get; set; }
    public int ReleaseDate { get; set; }
}