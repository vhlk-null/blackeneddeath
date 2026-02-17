using Library.API.Models.JoinTables;

namespace Library.API.Models
{
    public class Genre
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;

        public Guid? ParentGenreId { get;set;  }
        public Genre ParentGenre { get; set; }

        public ICollection<Genre> SubGenres { get; set; } = null!;

        public ICollection<AlbumGenre> Albums { get; set; } = null!;
        public ICollection<BandGenre> Bands { get; set; } = null!;
    }
}
