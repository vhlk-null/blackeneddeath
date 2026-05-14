namespace Library.Application.Search;

public record AlbumBandRef(Guid Id, string Name, string? Slug);

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
    List<string> Countries,
    List<string> Tracks,
    long CreatedAt,
    double? AverageRating = null,
    int RatingsCount = 0,
    bool IsExplicit = false,
    string? Label = null,
    int? ReleaseMonth = null,
    int? ReleaseDay = null);
