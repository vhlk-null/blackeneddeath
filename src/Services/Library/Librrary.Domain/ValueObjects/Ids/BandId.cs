namespace Library.Domain.ValueObjects.Ids;

public record BandId : EntityId<Guid>
{
    public static BandId Of(Guid value)
        => value == Guid.Empty ? throw new DomainException("BandId cannot be empty.") : Of<BandId>(value);
}
