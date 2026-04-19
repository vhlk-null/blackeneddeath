namespace BuildingBlocks.Storage;

public interface IStorageUrlResolver
{
    string? Resolve(string? publicId);
}
