namespace BuildingBlocks.Messaging.Events.Bands;

public class BandCreatedIntegrationEvent : IntegrationEvent
{
    public Guid BandId { get; init; }
    public string Name { get; init; } = null!;
    public string? Slug { get; init; }
    public string? Bio { get; init; }
    public string? LogoUrl { get; init; }
    public int? FormedYear { get; init; }
    public int? DisbandedYear { get; init; }
    public int Status { get; init; }
    public AlbumGenreInfo? PrimaryGenre { get; init; }
    public List<AlbumCountryInfo> Countries { get; init; } = [];
}
