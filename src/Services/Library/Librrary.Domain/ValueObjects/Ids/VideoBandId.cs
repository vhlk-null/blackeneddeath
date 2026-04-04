namespace Library.Domain.ValueObjects.Ids;

public record VideoBandId : EntityId<Guid>
{
    public static VideoBandId Of(Guid value)
        => value == Guid.Empty ? throw new DomainException("VideoBandId cannot be empty.") : Of<VideoBandId>(value);
}
