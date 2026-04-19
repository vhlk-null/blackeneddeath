using Microsoft.Extensions.Configuration;

namespace BuildingBlocks.Storage;

public class StorageUrlResolver(IConfiguration configuration) : IStorageUrlResolver
{
    private readonly string _baseUrl = configuration["Storage:BaseUrl"] ?? "https://res.cloudinary.com";
    private readonly string _cloudName = configuration["Storage:CloudName"]
        ?? throw new InvalidOperationException("Storage:CloudName is not configured.");

    public string? Resolve(string? publicId)
    {
        if (string.IsNullOrEmpty(publicId)) return null;
        if (publicId.StartsWith("http", StringComparison.OrdinalIgnoreCase)) return publicId;
        return $"{_baseUrl}/{_cloudName}/image/upload/{publicId}";
    }
}
