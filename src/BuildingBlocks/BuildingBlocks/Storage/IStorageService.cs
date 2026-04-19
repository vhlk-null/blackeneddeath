namespace BuildingBlocks.Storage;

public interface IStorageService
{
    Task<string> UploadFileAsync(string folder, string fileName, Stream stream, string contentType, CancellationToken cancellationToken = default);
    Task DeleteFileAsync(string publicId, CancellationToken cancellationToken = default);
}
