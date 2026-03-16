using Library.Domain.Events.Band;

namespace Library.Application.Bands.EventHandlers;

public class BandRemovedEventHandler(ILogger<BandRemovedEventHandler> logger) : INotificationHandler<BandRemovedEvent>
{
    public async ValueTask Handle(BandRemovedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event handled: {DomainEvent}", notification.GetType().Name);
    }
}
