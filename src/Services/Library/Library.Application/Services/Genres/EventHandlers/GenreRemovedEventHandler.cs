using Library.Domain.Events.Genre;

namespace Library.Application.Genres.EventHandlers;

public class GenreRemovedEventHandler(ILogger<GenreRemovedEventHandler> logger) : INotificationHandler<GenreRemovedEvent>
{
    public async ValueTask Handle(GenreRemovedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event handled: {DomainEvent}", notification.GetType().Name);
    }
}
