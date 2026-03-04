namespace Library.Domain.ValueObjects.Ids;

public record CountryId : EntityId<Guid>
{
    public static CountryId Of(Guid value)
        => value == Guid.Empty ? throw new DomainException("CountryId cannot be empty.") : Of<CountryId>(value);
}
