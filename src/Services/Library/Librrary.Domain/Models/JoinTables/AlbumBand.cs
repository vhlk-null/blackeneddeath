namespace Library.Domain.Models.JoinTables;

public class AlbumBand : JoinEntity
{
    public AlbumId AlbumId { get; private set; } = null!;
    public BandId BandId { get; private set; } = null!;
    public int Order { get; private set; }

    private AlbumBand() { }

    internal AlbumBand(AlbumId albumId, BandId bandId, int order)
    {
        AlbumId = albumId;
        BandId = bandId;
        Order = order;
    }

    internal void SetOrder(int order) => Order = order;
}
