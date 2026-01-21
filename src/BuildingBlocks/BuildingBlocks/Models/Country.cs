namespace BuildingBlocks.Models
{
    public class Country
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;

        public ICollection<Album> Albums { get; set; }
    }
}