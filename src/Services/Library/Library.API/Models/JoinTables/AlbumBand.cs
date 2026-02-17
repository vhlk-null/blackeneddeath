namespace Library.API.Models.JoinTables
{
    public class AlbumBand
    {
        public Guid AlbumId { get; set; }
        public Album Album { get; set; }

        public Guid BandId { get; set; }
        public Band Band { get; set; }

        public BandRole Role { get; set; }
    }

    public enum BandRole
    {
        Main,
        Split,
        Guest
    }
}
