namespace Library.Domain.ValueObjects.Ids;

public record GenreCardId : EntityId<Guid>
{
    public static GenreCardId Of(Guid value)
        => value == Guid.Empty ? throw new DomainException("GenreCardId cannot be empty.") : Of<GenreCardId>(value);
}
