namespace Library.Application.Search;

public record AlbumBandRef(Guid Id, string Name, string? Slug);
public record AlbumCountryRef(string Name, string? Code);

public record AlbumSearchDocument(
    string Id,
    string Title,
    string? Slug,
    string? CoverUrl,
    int ReleaseYear,
    string Type,
    string Format,
    List<AlbumBandRef> Bands,
    List<string> Genres,
    List<string> Tags,
    List<AlbumCountryRef> Countries,
    List<string> Tracks,
    long CreatedAt,
    double? AverageRating = null,
    int RatingsCount = 0);
