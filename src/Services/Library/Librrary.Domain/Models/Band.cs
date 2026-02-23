namespace Library.Domain.Models;

public class Band : Aggregate<BandId>
{
    public string Name { get; private set; } = null!;
    public string? Bio { get; private set; }
    public string? LogoUrl { get; private set; }
    public BandActivity Activity { get; private set; } = null!;
    public BandStatus Status { get; private set; }

    private readonly List<BandCountry> _bandCountries = new();

    public IReadOnlyList<BandCountry> BandCountries => _bandCountries.AsReadOnly();

    internal Band(string name, string? bio, string? logoUrl, BandActivity activity, BandStatus status)
    {
        Name = name;
        Bio = bio;
        LogoUrl = logoUrl;
        Activity = activity;
        Status = status;
    }
}
