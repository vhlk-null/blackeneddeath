namespace Library.Domain.ValueObjects.Ids;

public record GenreId : EntityId<Guid>
{
    public static GenreId Of(Guid value)
        => value == Guid.Empty ? throw new DomainException("GenreId cannot be empty.") : Of<GenreId>(value);
}
