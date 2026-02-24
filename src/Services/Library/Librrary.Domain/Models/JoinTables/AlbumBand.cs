namespace Library.Domain.Models.JoinTables;

public class AlbumBand : JoinEntity
{
    public AlbumId AlbumId { get; private set; } = null!;
    public BandId BandId { get; private set; } = null!;

    private AlbumBand() { }

    internal AlbumBand(AlbumId albumId, BandId bandId)
    {
        AlbumId = albumId;
        BandId = bandId;
    }
}
