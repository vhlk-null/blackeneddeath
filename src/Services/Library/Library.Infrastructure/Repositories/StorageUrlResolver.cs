using CloudinaryDotNet;
using Library.Application.Data;

namespace Library.Infrastructure.Repositories;

public class StorageUrlResolver(Cloudinary cloudinary) : IStorageUrlResolver
{
    public string? Resolve(string? key)
    {
        if (string.IsNullOrEmpty(key)) return null;
        if (key.StartsWith("http", StringComparison.OrdinalIgnoreCase)) return key;
        return cloudinary.Api.UrlImgUp.BuildUrl(key);
    }
}
