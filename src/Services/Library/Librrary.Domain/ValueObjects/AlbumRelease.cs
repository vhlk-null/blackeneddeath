namespace Library.Domain.ValueObjects;

public record AlbumRelease
{
    public int ReleaseYear { get; init; }
    public AlbumFormat Format { get; init; }
}
