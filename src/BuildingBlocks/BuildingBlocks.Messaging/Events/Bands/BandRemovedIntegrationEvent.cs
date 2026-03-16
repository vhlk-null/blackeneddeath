namespace BuildingBlocks.Messaging.Events.Bands;

public class BandRemovedIntegrationEvent : IntegrationEvent
{
    public Guid BandId { get; init; }
    public string Name { get; init; } = null!;
}
