using Library.Domain.Events.Genre;

namespace Library.Application.Genres.EventHandlers;

public class GenreCreatedEventHandler(ILogger<GenreCreatedEventHandler> logger) : INotificationHandler<GenreCreatedEvent>
{
    public async ValueTask Handle(GenreCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event handled: {DomainEvent}", notification.GetType().Name);
    }
}
