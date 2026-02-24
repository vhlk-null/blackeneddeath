namespace Library.Domain.ValueObjects.Ids;

public record AlbumId : EntityId<Guid>
{
    public static AlbumId Of(Guid value)
        => value == Guid.Empty ? throw new DomainException("AlbumId cannot be empty.") : Of<AlbumId>(value);
}
