namespace Library.Application.Search;

public static class SearchIndexes
{
    public const string Albums = "albums";
    public const string Bands = "bands";

    public static class AlbumAttributes
    {
        public static readonly string[] Filterable = ["genres", "tags", "countries.name", "type", "format", "releaseYear", "averageRating", "ratingsCount"];
        public static readonly string[] Sortable = ["title", "releaseYear", "createdAt", "averageRating", "ratingsCount"];
        public static readonly string[] Searchable = ["title", "tracks"];
    }

    public static class BandAttributes
    {
        public static readonly string[] Filterable = ["genres", "countries", "status", "formedYear", "averageRating", "ratingsCount"];
        public static readonly string[] Sortable = ["name", "formedYear", "createdAt", "averageRating", "ratingsCount"];
        public static readonly string[] Searchable = ["name"];
    }
}
