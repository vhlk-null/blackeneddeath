namespace Library.Domain.Events.Genre;

public record GenreCreatedEvent(Models.Genre Genre) : IDomainEvent;
