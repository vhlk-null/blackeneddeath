namespace Library.Infrastructure.Repositories;

public class CloudinaryStorageService(Cloudinary cloudinary, string environmentPrefix) : IStorageService
{
    public async Task<string> UploadFileAsync(string folder, string fileName, Stream stream, string contentType, CancellationToken cancellationToken = default)
    {
        string publicId = Path.GetFileNameWithoutExtension(fileName);

        ImageUploadParams uploadParams = new ImageUploadParams
        {
            File = new FileDescription(fileName, stream),
            Folder = $"{environmentPrefix}/{folder}",
            PublicId = publicId
        };

        ImageUploadResult? result = await cloudinary.UploadAsync(uploadParams);

        return result.PublicId;
    }

    public async Task DeleteFileAsync(string publicId, CancellationToken cancellationToken = default)
    {
        DeletionParams deleteParams = new DeletionParams(publicId);
        await cloudinary.DestroyAsync(deleteParams);
    }
}
