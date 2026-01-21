namespace BuildingBlocks.Models.JoinTables
{
    public class AlbumBand
    {
        public Guid AlbumId { get; set; }
        public Album Album { get; set; }

        public Guid BandId { get; set; }
        public Band Band { get; set; }
    }
}
