namespace Library.Domain.Models.JoinTables;

public class GenreCardGenre : JoinEntity
{
    public GenreCardId GenreCardId { get; private set; } = null!;
    public GenreId GenreId { get; private set; } = null!;

    private GenreCardGenre() { }

    internal GenreCardGenre(GenreCardId genreCardId, GenreId genreId)
    {
        GenreCardId = genreCardId;
        GenreId = genreId;
    }
}
