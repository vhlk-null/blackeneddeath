namespace Library.Application.Search;

public static class SearchIndexes
{
    public const string Albums = "albums";
    public const string Bands = "bands";

    public static class AlbumAttributes
    {
        public static readonly string[] Filterable = ["genres", "tags", "countries", "type", "format", "releaseYear"];
        public static readonly string[] Sortable = ["title", "releaseYear", "createdAt"];
        public static readonly string[] Searchable = ["title", "tracks"];
    }

    public static class BandAttributes
    {
        public static readonly string[] Filterable = ["genres", "countries", "status", "formedYear"];
        public static readonly string[] Sortable = ["name", "formedYear", "createdAt"];
        public static readonly string[] Searchable = ["name"];
    }
}
