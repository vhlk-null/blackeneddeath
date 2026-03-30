namespace Library.Domain.Events.GenreCard;

public record GenreCardRemovedEvent(Models.GenreCard GenreCard) : IDomainEvent;
