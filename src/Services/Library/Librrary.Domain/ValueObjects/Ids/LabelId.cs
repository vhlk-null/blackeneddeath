namespace Library.Domain.ValueObjects.Ids;

public record LabelId : EntityId<Guid>
{
    public static LabelId Of(Guid value)
        => value == Guid.Empty ? throw new DomainException("LabelId cannot be empty.") : Of<LabelId>(value);
}
