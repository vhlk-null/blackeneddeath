namespace Library.API.Constants;

public static class OutputCachePolicies
{
    public const string AlbumBySlug = "AlbumBySlug";
}

public static class OutputCacheTags
{
    public static string Album(string slug) => $"album:{slug}";
}
