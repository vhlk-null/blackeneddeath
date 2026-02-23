namespace Library.Domain.Models;

public class Album : Aggregate<AlbumId>
{
    public string Title { get; private set; } = null!;
    public AlbumType Type { get; private set; }
    public AlbumRelease AlbumRelease { get; private set; } = null!;
    public string? CoverUrl { get; private set; }
    public LabelInfo? LabelInfo { get; private set; }
    
    private readonly List<AlbumBand> _albumBands = new();
    private readonly List<AlbumGenre> _albumGenres = new();
    private readonly List<AlbumCountry> _albumCountries = new();
    private readonly List<Track> _tracks = new();
    private readonly List<StreamingLink> _streamingLinks = new();

    public IReadOnlyList<AlbumBand> AlbumBands => _albumBands.AsReadOnly();
    public IReadOnlyList<AlbumGenre> AlbumGenres => _albumGenres.AsReadOnly();
    public IReadOnlyList<AlbumCountry> AlbumCountries => _albumCountries.AsReadOnly();
    public IReadOnlyList<Track> Tracks => _tracks.AsReadOnly();
    public IReadOnlyList<StreamingLink> StreamingLinks => _streamingLinks.AsReadOnly();
}