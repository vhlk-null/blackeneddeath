namespace Library.Domain.Models;

public class Tag : Entity<TagId>
{
    public string Name { get; private set; } = null!;

    private Tag() { }

    public static Tag Create(TagId id, string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return new Tag { Id = id, Name = name };
    }

    public Tag Update(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        Name = name;

        return this;
    }
}
