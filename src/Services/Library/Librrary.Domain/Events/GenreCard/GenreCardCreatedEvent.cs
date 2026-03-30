namespace Library.Domain.Events.GenreCard;

public record GenreCardCreatedEvent(Models.GenreCard GenreCard) : IDomainEvent;
