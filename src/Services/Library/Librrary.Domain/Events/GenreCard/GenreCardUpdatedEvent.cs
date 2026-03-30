namespace Library.Domain.Events.GenreCard;

public record GenreCardUpdatedEvent(Models.GenreCard GenreCard) : IDomainEvent;
