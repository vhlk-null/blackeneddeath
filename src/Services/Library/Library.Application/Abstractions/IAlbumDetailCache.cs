namespace Library.Application.Abstractions;

public interface IAlbumDetailCache
{
    Task<AlbumDetailDto?> GetAsync(string slug, CancellationToken cancellationToken = default);
    Task SetAsync(string slug, IReadOnlyList<Guid> bandIds, AlbumDetailDto dto, CancellationToken cancellationToken = default);
    Task InvalidateForBandsAsync(IEnumerable<Guid> bandIds, CancellationToken cancellationToken = default);
    Task InvalidateBySlugAsync(string slug, CancellationToken cancellationToken = default);
}
