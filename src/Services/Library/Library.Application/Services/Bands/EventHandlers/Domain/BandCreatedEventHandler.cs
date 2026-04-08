namespace Library.Application.Services.Bands.EventHandlers.Domain;

public sealed class BandCreatedEventHandler(ILogger<BandCreatedEventHandler> logger, IPublishEndpoint publishEndpoint)
    : INotificationHandler<BandCreatedEvent>
{
    public async ValueTask Handle(BandCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event handled: {DomainEvent}", domainEvent.GetType().Name);

        BandCreatedIntegrationEvent integrationEvent = new BandCreatedIntegrationEvent
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
