namespace Library.Domain.Models;

public class VideoBand : Entity<VideoBandId>
{
    public BandId BandId { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public int? Year { get; private set; }
    public CountryId? CountryId { get; private set; }
    public VideoType VideoType { get; private set; }
    public string YoutubeLink { get; private set; } = null!;
    public string? Info { get; private set; }
    public bool IsApproved { get; private set; }

    private VideoBand() { }

    public static VideoBand Create(BandId bandId, string name, int? year, CountryId? countryId,
        VideoType videoType, string youtubeLink, string? info, VideoBandId? id = null)
    {
        ArgumentNullException.ThrowIfNull(bandId);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(youtubeLink);

        return new VideoBand
        {
            Id = id ?? VideoBandId.Of(Guid.NewGuid()),
            BandId = bandId,
            Name = name,
            Year = year,
            CountryId = countryId,
            VideoType = videoType,
            YoutubeLink = youtubeLink,
            Info = info
        };
    }

    public void Approve()
    {
        IsApproved = true;
    }

    public void Update(string name, int? year, CountryId? countryId,
        VideoType videoType, string youtubeLink, string? info)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(youtubeLink);

        Name = name;
        Year = year;
        CountryId = countryId;
        VideoType = videoType;
        YoutubeLink = youtubeLink;
        Info = info;
    }
}
