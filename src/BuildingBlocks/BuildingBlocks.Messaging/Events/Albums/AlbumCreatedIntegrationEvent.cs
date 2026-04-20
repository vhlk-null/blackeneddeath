using BuildingBlocks.Messaging.Events;

namespace BuildingBlocks.Messaging.Events.Albums;

public class AlbumCreatedIntegrationEvent : IntegrationEvent
{
    public Guid AlbumId { get; init; }
    public string Title { get; init; } = null!;
    public string? Slug { get; init; }
    public string? CoverUrl { get; init; }
    public int ReleaseYear { get; init; }
    public int Format { get; init; }
    public int Type { get; init; }
    public string? Label { get; init; }
    public List<AlbumBandInfo> Bands { get; init; } = [];
    public AlbumGenreInfo? PrimaryGenre { get; init; }
    public List<AlbumCountryInfo> Countries { get; init; } = [];
    public bool IsExplicit { get; init; }
}