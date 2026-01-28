namespace BuildingBlocks.Models.JoinTables
{
    public class AlbumCountry
    {
        public Guid AlbumId { get; set; }
        public Album Album { get; set; } = default!;

        public Guid CountryId { get; set; }
        public Country Country { get; set; } = default!;
    }
}
