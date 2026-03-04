namespace Library.Domain.Events.Genre;

public record GenreUpdatedEvent(Models.Genre Genre) : IDomainEvent;
