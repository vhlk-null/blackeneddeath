namespace Library.Application.Search;

public record AlbumSearchDocument(
    string Id,
    string Title,
    string? Slug,
    string? CoverUrl,
    int ReleaseYear,
    string Type,
    string Format,
    List<string> Bands,
    List<string> Genres,
    List<string> Tags,
    List<string> Countries,
    List<string> Tracks,
    long CreatedAt,
    double? AverageRating = null,
    int RatingsCount = 0);
