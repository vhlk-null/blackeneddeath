namespace Library.Application.Services.Bands.EventHandlers.Domain;

public sealed class BandRemovedEventHandler(ILogger<BandRemovedEventHandler> logger, IPublishEndpoint publishEndpoint)
    : INotificationHandler<BandRemovedEvent>
{
    public async ValueTask Handle(BandRemovedEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event handled: {DomainEvent}", domainEvent.GetType().Name);

        BandRemovedIntegrationEvent integrationEvent = new BandRemovedIntegrationEvent
        {
            BandId = domainEvent.Band.Id.Value,
            Name = domainEvent.Band.Name
        };

        await publishEndpoint.Publish(integrationEvent, cancellationToken);
    }
}
