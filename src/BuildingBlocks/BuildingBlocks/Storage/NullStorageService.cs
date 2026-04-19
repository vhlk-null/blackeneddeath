namespace BuildingBlocks.Storage;

public class NullStorageService : IStorageService
{
    public Task<string> UploadFileAsync(string folder, string fileName, Stream stream, string contentType, CancellationToken cancellationToken = default)
        => Task.FromResult(string.Empty);

    public Task DeleteFileAsync(string publicId, CancellationToken cancellationToken = default)
        => Task.CompletedTask;
}
