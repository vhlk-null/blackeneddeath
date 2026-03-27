namespace Library.Domain.Models;

public class Country : Entity<CountryId>
{
    public string Name { get; private set; } = null!;
    public string? Code { get; private set; }

    private Country() { }

    public static Country Create(CountryId id, string name, string? code)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return new Country { Id = id, Name = name, Code = code };
    }

    public Country Update(string name, string? code)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        Name = name;
        Code = code;

        return this;
    }
}