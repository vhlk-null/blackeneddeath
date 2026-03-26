namespace Library.Domain.Models.JoinTables;

public class AlbumTag : JoinEntity
{
    public AlbumId AlbumId { get; private set; } = null!;
    public TagId TagId { get; private set; } = null!;

    private AlbumTag() { }

    internal AlbumTag(AlbumId albumId, TagId tagId)
    {
        AlbumId = albumId;
        TagId = tagId;
    }
}
