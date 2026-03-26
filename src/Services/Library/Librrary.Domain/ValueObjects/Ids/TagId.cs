namespace Library.Domain.ValueObjects.Ids;

public record TagId : EntityId<Guid>
{
    public static TagId Of(Guid value)
        => value == Guid.Empty ? throw new DomainException("TagId cannot be empty.") : Of<TagId>(value);
}
