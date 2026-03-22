using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Library.Application.Data;

namespace Library.Infrastructure.Repositories;

public class CloudinaryStorageService(Cloudinary cloudinary) : IStorageService
{
    public async Task<string> UploadFileAsync(string folder, string fileName, Stream stream, string contentType, CancellationToken cancellationToken = default)
    {
        var publicId = Path.GetFileNameWithoutExtension(fileName);

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(fileName, stream),
            Folder = folder,
            PublicId = publicId
        };

        var result = await cloudinary.UploadAsync(uploadParams);

        return result.PublicId;
    }
}
