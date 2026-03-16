using Library.Domain.Events.Band;

namespace Library.Application.Bands.EventHandlers;

public class BandUpdatedEventHandler(ILogger<BandUpdatedEventHandler> logger) : INotificationHandler<BandUpdatedEvent>
{
    public async ValueTask Handle(BandUpdatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event handled: {DomainEvent}", notification.GetType().Name);
    }
}
