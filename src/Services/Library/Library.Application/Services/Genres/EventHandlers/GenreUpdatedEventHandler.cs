using Library.Domain.Events.Genre;

namespace Library.Application.Genres.EventHandlers;

public class GenreUpdatedEventHandler(ILogger<GenreUpdatedEventHandler> logger) : INotificationHandler<GenreUpdatedEvent>
{
    public async ValueTask Handle(GenreUpdatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event handled: {DomainEvent}", notification.GetType().Name);
    }
}
