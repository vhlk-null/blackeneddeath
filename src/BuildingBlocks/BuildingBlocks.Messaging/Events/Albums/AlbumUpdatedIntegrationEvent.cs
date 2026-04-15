namespace BuildingBlocks.Messaging.Events.Albums;

public class AlbumUpdatedIntegrationEvent : IntegrationEvent
{
    public Guid AlbumId { get; set; }
    public string Title { get; set; } = null!;
    public string? Slug { get; set; }
    public string? CoverUrl { get; set; }
    public int ReleaseDate { get; set; }
    public int Format { get; set; }
    public int Type { get; set; }
    public List<AlbumBandInfo> Bands { get; set; } = [];
    public AlbumGenreInfo? PrimaryGenre { get; set; }
    public List<AlbumCountryInfo> Countries { get; set; } = [];
}