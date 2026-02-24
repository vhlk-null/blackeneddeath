namespace Library.Domain.Events.Band;

public record BandUpdatedEvent(Models.Band Band) : IDomainEvent;
