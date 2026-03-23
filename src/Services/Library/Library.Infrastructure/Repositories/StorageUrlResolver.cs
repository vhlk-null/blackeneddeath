namespace Library.Infrastructure.Repositories;

public class StorageUrlResolver(IConfiguration configuration) : IStorageUrlResolver
{
    private readonly string _baseUrl = configuration["Storage:BaseUrl"]
        ?? throw new InvalidOperationException("Storage:BaseUrl is not configured.");
    private readonly string _cloudName = configuration["Storage:CloudName"]
        ?? throw new InvalidOperationException("Storage:CloudName is not configured.");

    public string? Resolve(string? key)
    {
        if (string.IsNullOrEmpty(key)) return null;
        if (key.StartsWith("http", StringComparison.OrdinalIgnoreCase)) return key;
        return $"{_baseUrl}/{_cloudName}/image/upload/{key}";
    }
}
