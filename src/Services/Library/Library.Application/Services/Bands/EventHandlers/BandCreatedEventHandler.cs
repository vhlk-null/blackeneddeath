using Library.Domain.Events.Band;

namespace Library.Application.Services.Bands.EventHandlers;

public class BandCreatedEventHandler(ILogger<BandCreatedEventHandler> logger) : INotificationHandler<BandCreatedEvent>
{
    public async ValueTask Handle(BandCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event handled: {DomainEvent}", notification.GetType().Name);
    }
}
