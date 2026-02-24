namespace Library.Domain.ValueObjects.Ids;

public record TrackId : EntityId<Guid>
{
    public static TrackId Of(Guid value) 
        => value == Guid.Empty ? throw new DomainException("TrackId cannot be empty.") : Of<TrackId>(value);
}
