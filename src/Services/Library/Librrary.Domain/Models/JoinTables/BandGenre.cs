namespace Library.Domain.Models.JoinTables;

public class BandGenre : JoinEntity
{
    public BandId BandId { get; private set; } = null!;
    public GenreId GenreId { get; private set; } = null!;
    public bool IsPrimary { get; internal set; }

    private BandGenre() { }

    internal BandGenre(BandId bandId, GenreId genreId, bool isPrimary)
    {
        BandId = bandId;
        GenreId = genreId;
        IsPrimary = isPrimary;
    }
}
