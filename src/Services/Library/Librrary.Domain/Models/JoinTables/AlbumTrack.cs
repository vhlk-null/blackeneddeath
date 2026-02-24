namespace Library.Domain.Models.JoinTables;

public class AlbumTrack : JoinEntity
{
    public AlbumId AlbumId { get; private set; } = null!;
    public TrackId TrackId { get; private set; } = null!;
    public int TrackNumber { get; private set; }

    private AlbumTrack() { }

    internal AlbumTrack(AlbumId albumId, TrackId trackId, int trackNumber)
    {
        AlbumId = albumId;
        TrackId = trackId;
        TrackNumber = trackNumber;
    }
}
