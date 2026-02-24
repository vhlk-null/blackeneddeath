namespace Library.Domain.Events.Band;

public record BandCreatedEvent(Models.Band Band) : IDomainEvent;
