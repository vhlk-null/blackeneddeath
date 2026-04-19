using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace BuildingBlocks.Storage;

public class CloudinaryStorageService(Cloudinary cloudinary, string environmentPrefix) : IStorageService
{
    public async Task<string> UploadFileAsync(string folder, string fileName, Stream stream, string contentType, CancellationToken cancellationToken = default)
    {
        ImageUploadParams uploadParams = new ImageUploadParams
        {
            File = new FileDescription(fileName, stream),
            Folder = $"{environmentPrefix}/{folder}",
            PublicId = Path.GetFileNameWithoutExtension(fileName)
        };

        ImageUploadResult result = await cloudinary.UploadAsync(uploadParams);
        return result.PublicId;
    }

    public async Task DeleteFileAsync(string publicId, CancellationToken cancellationToken = default)
    {
        await cloudinary.DestroyAsync(new DeletionParams(publicId));
    }
}
