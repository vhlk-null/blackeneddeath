
namespace Library.Domain.Models
{
    public class Country : Entity<Guid>
    {
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
    }
}