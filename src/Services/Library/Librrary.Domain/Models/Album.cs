namespace Library.Domain.Models;

public class Album : Aggregate<AlbumId>
{
    public string Title { get; private set; } = null!;
    public AlbumType Type { get; private set; }
    public AlbumRelease AlbumRelease { get; private set; } = null!;
    public string? CoverUrl { get; private set; }
    public LabelInfo? LabelInfo { get; private set; }
    
    private readonly List<AlbumBand> _albumBands = [];
    private readonly List<AlbumGenre> _albumGenres = [];
    private readonly List<AlbumCountry> _albumCountries = [];
    private readonly List<AlbumTrack> _albumTracks = [];
    private readonly List<StreamingLink> _streamingLinks = [];

    public IReadOnlyList<AlbumBand> AlbumBands => _albumBands.AsReadOnly();
    public IReadOnlyList<AlbumGenre> AlbumGenres => _albumGenres.AsReadOnly();
    public IReadOnlyList<AlbumCountry> AlbumCountries => _albumCountries.AsReadOnly();
    public IReadOnlyList<AlbumTrack> AlbumTracks => _albumTracks.AsReadOnly();
    public IReadOnlyList<StreamingLink> StreamingLinks => _streamingLinks.AsReadOnly();

    private Album() { }

    public static Album Create(string title, AlbumType type, AlbumRelease albumRelease, string? coverUrl, LabelInfo? labelInfo)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentNullException.ThrowIfNull(albumRelease);

        var album =  new Album
        {
            Id = AlbumId.Of(Guid.NewGuid()),
            Title = title,
            Type = type,
            AlbumRelease = albumRelease,
            CoverUrl = coverUrl,
            LabelInfo = labelInfo
        };

        album.AddDomainEvent(new AlbumCreatedEvent(album));

        return album;
    }

    public Album Update(string title, AlbumType type, AlbumRelease albumRelease, string? coverUrl, LabelInfo? labelInfo)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentNullException.ThrowIfNull(albumRelease);

        Title = title;
        Type = type;
        AlbumRelease = albumRelease;
        CoverUrl = coverUrl;
        LabelInfo = labelInfo;

        AddDomainEvent(new AlbumUpdatedEvent(this));

        return this;
    }

    public void AddBand(BandId bandId)
    {
        ArgumentNullException.ThrowIfNull(bandId);
        if (_albumBands.Any(x => x.BandId == bandId))
            throw new DomainException("Band is already associated with this album.");

        _albumBands.Add(new AlbumBand(Id, bandId));
    }

    public void RemoveBand(BandId bandId)
    {
        var entry = _albumBands.FirstOrDefault(x => x.BandId == bandId)
            ?? throw new DomainException("Band is not associated with this album.");

        _albumBands.Remove(entry);
    }

    public void AddGenre(GenreId genreId, bool isPrimary)
    {
        ArgumentNullException.ThrowIfNull(genreId);
        if (_albumGenres.Any(x => x.GenreId == genreId))
            throw new DomainException("Genre is already associated with this album.");

        _albumGenres.Add(new AlbumGenre(Id, genreId, isPrimary));
    }

    public void RemoveGenre(GenreId genreId)
    {
        var entry = _albumGenres.FirstOrDefault(x => x.GenreId == genreId)
            ?? throw new DomainException("Genre is not associated with this album.");

        _albumGenres.Remove(entry);
    }

    public void AddCountry(CountryId countryId)
    {
        ArgumentNullException.ThrowIfNull(countryId);
        if (_albumCountries.Any(x => x.CountryId == countryId))
            throw new DomainException("Country is already associated with this album.");

        _albumCountries.Add(new AlbumCountry(Id, countryId));
    }

    public void RemoveCountry(CountryId countryId)
    {
        var entry = _albumCountries.FirstOrDefault(x => x.CountryId == countryId)
            ?? throw new DomainException("Country is not associated with this album.");

        _albumCountries.Remove(entry);
    }

    public void AddTrack(TrackId trackId, int trackNumber)
    {
        ArgumentNullException.ThrowIfNull(trackId);
        if (_albumTracks.Any(x => x.TrackId == trackId))
            throw new DomainException("Track is already associated with this album.");

        _albumTracks.Add(new AlbumTrack(Id, trackId, trackNumber));
    }

    public void RemoveTrack(TrackId trackId)
    {
        var entry = _albumTracks.FirstOrDefault(x => x.TrackId == trackId)
            ?? throw new DomainException("Track is not associated with this album.");

        _albumTracks.Remove(entry);
    }

    public void AddStreamingLink(StreamingPlatform platform, string embedCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(embedCode);
        if (_streamingLinks.Any(x => x.Platform == platform))
            throw new DomainException("A streaming link for this platform already exists.");

        _streamingLinks.Add(new StreamingLink(platform, embedCode));
    }

    public void RemoveStreamingLink(StreamingLinkId streamingLinkId)
    {
        var entry = _streamingLinks.FirstOrDefault(x => x.Id == streamingLinkId)
            ?? throw new DomainException("Streaming link not found.");

        _streamingLinks.Remove(entry);
    }
}