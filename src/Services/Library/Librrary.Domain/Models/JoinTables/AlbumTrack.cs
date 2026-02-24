namespace Library.Domain.Models.JoinTables;

public class AlbumTrack : JoinEntity
{
    public AlbumId AlbumId { get; private set; }
    public TrackId TrackId { get; private set; }
    public int TrackNumber { get; private set; }

    private AlbumTrack() { }

    internal AlbumTrack(AlbumId albumId, TrackId trackId, int trackNumber)
    {
        AlbumId = albumId;
        TrackId = trackId;
        TrackNumber = trackNumber;
    }
}
