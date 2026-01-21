using Archive.API.Models.JoinTables;

namespace Archive.API.Models
{
    public class Tag
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ICollection<AlbumTag> Albums { get; set; }
    }
}
