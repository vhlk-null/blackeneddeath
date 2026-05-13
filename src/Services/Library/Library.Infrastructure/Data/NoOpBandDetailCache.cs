using Library.Application.Abstractions;
using Library.Application.Dtos;

namespace Library.Infrastructure.Data;

public class NoOpBandDetailCache : IBandDetailCache
{
    public Task<BandDetailDto?> GetAsync(string slug, CancellationToken cancellationToken = default) =>
        Task.FromResult<BandDetailDto?>(null);

    public Task SetAsync(string slug, Guid bandId, BandDetailDto dto, CancellationToken cancellationToken = default) =>
        Task.CompletedTask;

    public Task InvalidateAsync(Guid bandId, CancellationToken cancellationToken = default) =>
        Task.CompletedTask;
}
