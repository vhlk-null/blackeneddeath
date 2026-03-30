namespace Library.Domain.Models.JoinTables;

public class GenreCardTag : JoinEntity
{
    public GenreCardId GenreCardId { get; private set; } = null!;
    public TagId TagId { get; private set; } = null!;

    private GenreCardTag() { }

    internal GenreCardTag(GenreCardId genreCardId, TagId tagId)
    {
        GenreCardId = genreCardId;
        TagId = tagId;
    }
}
