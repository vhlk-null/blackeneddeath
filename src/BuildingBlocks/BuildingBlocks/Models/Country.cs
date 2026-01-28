using BuildingBlocks.Models.JoinTables;

namespace BuildingBlocks.Models
{
    public class Country
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;

        public List<Band> Bands { get; set; }
        public List<AlbumCountry> Albums { get; set; }
    }
}