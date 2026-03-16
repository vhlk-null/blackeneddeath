namespace BuildingBlocks.Messaging.Events.Albums;

public class AlbumCreatedIntegrationEvent : IntegrationEvent
{
    public Guid AlbumId { get; init; }
    public string Title { get; init; } = null!;
    public string? CoverUrl { get; init; }
    public int ReleaseYear { get; init; }
    public int Format { get; init; }
    public int Type { get; init; }
    public string? Label { get; init; }
}