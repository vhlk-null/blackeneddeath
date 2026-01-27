namespace Archive.API.Models
{
    public class Band
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Bio { get; set; }

        public Guid? CountryId { get; set; }
        public Country? Country { get; set; }

        public string? LogoUrl { get; set; }

        public int? FormedYear { get; set; }
        public int? DisbandedYear { get; set; }
        public BandStatus Status { get; set; }

        public ICollection<AlbumBand> Albums { get; set; }
        public ICollection<BandGenre> Genres { get; set; }
    }
}
