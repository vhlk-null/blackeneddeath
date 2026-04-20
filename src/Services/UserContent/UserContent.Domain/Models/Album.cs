namespace UserContent.Domain.Models;

public class Album
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public string? Slug { get; set; }
    public string? CoverUrl { get; set; }
    public int ReleaseDate { get; set; }
    public int Format { get; set; }
    public int Type { get; set; }
    public string? PrimaryGenreName { get; set; }
    public string? PrimaryGenreSlug { get; set; }
    public string? BandIds { get; set; }
    public string? BandNames { get; set; }
    public string? BandSlugs { get; set; }
    public string? CountryNames { get; set; }
    public string? CountryCodes { get; set; }
    public double? AverageRating { get; set; }
    public int RatingsCount { get; set; }
    public bool IsExplicit { get; set; }
    public List<FavoriteAlbum> FavoriteAlbums { get; set; } = new();
    public List<AlbumReview> Reviews { get; set; } = new();
    public List<CollectionAlbum> CollectionAlbums { get; set; } = new();
}