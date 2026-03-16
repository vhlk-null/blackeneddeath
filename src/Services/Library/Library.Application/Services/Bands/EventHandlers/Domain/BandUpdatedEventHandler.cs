namespace Library.Application.Services.Bands.EventHandlers.Domain;

public sealed class BandUpdatedEventHandler(ILogger<BandUpdatedEventHandler> logger, IPublishEndpoint publishEndpoint)
    : INotificationHandler<BandUpdatedEvent>
{
    public async ValueTask Handle(BandUpdatedEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event handled: {DomainEvent}", domainEvent.GetType().Name);

        var integrationEvent = new BandUpdatedIntegrationEvent
        {
            BandId = domainEvent.Band.Id.Value,
            Name = domainEvent.Band.Name,
            Bio = domainEvent.Band.Bio,
            LogoUrl = domainEvent.Band.LogoUrl,
            FormedYear = domainEvent.Band.Activity.FormedYear,
            DisbandedYear = domainEvent.Band.Activity.DisbandedYear,
            Status = (int)domainEvent.Band.Status
        };

        await publishEndpoint.Publish(integrationEvent, cancellationToken);
    }
}
