namespace Library.Domain.Events.Genre;

public record GenreRemovedEvent(Models.Genre Genre) : IDomainEvent;
