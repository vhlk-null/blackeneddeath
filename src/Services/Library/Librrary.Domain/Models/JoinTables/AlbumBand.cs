namespace Library.Domain.Models.JoinTables
{
    public class AlbumBand : JoinEntity
    {
        public AlbumId AlbumId { get; private set; }
        public BandId BandId { get; private set; }

        private AlbumBand() { }

        internal AlbumBand(AlbumId albumId, BandId bandId)
        {
            AlbumId = albumId;
            BandId = bandId;
        }
    }
}
