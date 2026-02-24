namespace Library.Domain.Models;

public class Band : Aggregate<BandId>
{
    public string Name { get; private set; } = null!;
    public string? Bio { get; private set; }
    public string? LogoUrl { get; private set; }
    public BandActivity Activity { get; private set; } = null!;
    public BandStatus Status { get; private set; }

    private readonly List<BandCountry> _bandCountries = [];

    public IReadOnlyList<BandCountry> BandCountries => _bandCountries.AsReadOnly();

    private Band() { }

    public static Band Create(string name, string? bio, string? logoUrl, BandActivity activity, BandStatus status)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(activity);

        var band = new Band
        {
            Id = BandId.Of(Guid.NewGuid()),
            Name = name,
            Bio = bio,
            LogoUrl = logoUrl,
            Activity = activity,
            Status = status
        };

        band.AddDomainEvent(new BandCreatedEvent(band));

        return band;
    }

    public Band Update(string name, string? bio, string? logoUrl, BandActivity activity, BandStatus status)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(activity);

        Name = name;
        Bio = bio;
        LogoUrl = logoUrl;
        Activity = activity;
        Status = status;

        AddDomainEvent(new BandUpdatedEvent(this));

        return this;
    }

    public void Remove()
    {
        AddDomainEvent(new BandRemovedEvent(this));
    }

    public void AddCountry(CountryId countryId)
    {
        ArgumentNullException.ThrowIfNull(countryId);
        if (_bandCountries.Any(x => x.CountryId == countryId))
            throw new DomainException("Country is already associated with this band.");

        _bandCountries.Add(new BandCountry(countryId, Id));
    }

    public void RemoveCountry(CountryId countryId)
    {
        var entry = _bandCountries.FirstOrDefault(x => x.CountryId == countryId)
            ?? throw new DomainException("Country is not associated with this band.");

        _bandCountries.Remove(entry);
    }
}
