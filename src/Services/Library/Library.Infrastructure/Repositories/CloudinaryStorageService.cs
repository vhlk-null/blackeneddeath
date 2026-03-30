namespace Library.Infrastructure.Repositories;

public class CloudinaryStorageService(Cloudinary cloudinary, string environmentPrefix) : IStorageService
{
    public async Task<string> UploadFileAsync(string folder, string fileName, Stream stream, string contentType, CancellationToken cancellationToken = default)
    {
        var publicId = Path.GetFileNameWithoutExtension(fileName);

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(fileName, stream),
            Folder = $"{environmentPrefix}/{folder}",
            PublicId = publicId
        };

        var result = await cloudinary.UploadAsync(uploadParams);

        return result.PublicId;
    }

    public async Task DeleteFileAsync(string publicId, CancellationToken cancellationToken = default)
    {
        var deleteParams = new DeletionParams(publicId);
        await cloudinary.DestroyAsync(deleteParams);
    }
}
