using Library.Application.Abstractions;
using Library.Application.Dtos;

namespace Library.Infrastructure.Data;

public class NoOpAlbumDetailCache : IAlbumDetailCache
{
    public Task<AlbumDetailDto?> GetAsync(string slug, CancellationToken cancellationToken = default) =>
        Task.FromResult<AlbumDetailDto?>(null);

    public Task SetAsync(string slug, IReadOnlyList<Guid> bandIds, AlbumDetailDto dto, CancellationToken cancellationToken = default) =>
        Task.CompletedTask;

    public Task InvalidateForBandsAsync(IEnumerable<Guid> bandIds, CancellationToken cancellationToken = default) =>
        Task.CompletedTask;

    public Task InvalidateBySlugAsync(string slug, CancellationToken cancellationToken = default) =>
        Task.CompletedTask;
}
