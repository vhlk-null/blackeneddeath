namespace BuildingBlocks.Messaging.Events.Bands;

public class BandUpdatedIntegrationEvent : IntegrationEvent
{
    public Guid BandId { get; set; }
    public string Name { get; set; } = null!;
    public string? Bio { get; set; }
    public string? LogoUrl { get; set; }
    public int? FormedYear { get; set; }
    public int? DisbandedYear { get; set; }
    public int Status { get; set; }
}
