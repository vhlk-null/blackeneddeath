namespace Library.Domain.Events.Band;

public record BandRemovedEvent(Models.Band Band) : IDomainEvent;
