namespace UserContent.Application.Abstractions;

public interface ILibraryService
{
    Task<Album?> GetAlbumByIdAsync(Guid albumId, CancellationToken ct = default);
}
