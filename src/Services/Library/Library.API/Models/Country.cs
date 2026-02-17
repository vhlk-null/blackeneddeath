using Library.API.Models.JoinTables;

namespace Library.API.Models
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