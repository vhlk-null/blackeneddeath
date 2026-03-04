namespace Library.Domain.ValueObjects.Ids;

public record StreamingLinkId : EntityId<Guid>
{
    public static StreamingLinkId Of(Guid value)
        => value == Guid.Empty ? throw new DomainException("StreamingLinkId cannot be empty.") : Of<StreamingLinkId>(value);
}
