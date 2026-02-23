namespace Library.Domain.Models.JoinTables
{
    public class AlbumBand : JoinEntity
    {
        public AlbumId AlbumId { get; private set; }
        public BandId BandId { get; private set; }

        internal AlbumBand(AlbumId albumId, BandId bandId)
        {
            AlbumId = albumId;
            BandId = bandId;
        }
    }
}
