namespace Library.Domain.Models.JoinTables;

public class AlbumGenre : JoinEntity
{
    public AlbumId AlbumId { get; private set; } = null!;
    public GenreId GenreId { get; private set; } = null!;
    public bool IsPrimary { get; private set; }

    private AlbumGenre() { }

    internal AlbumGenre(AlbumId albumId, GenreId genreId, bool isPrimary)
    {
        AlbumId = albumId;
        GenreId = genreId;
        IsPrimary = isPrimary;
    }
}
