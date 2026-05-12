using Library.Application.Abstractions;
using Microsoft.Extensions.Caching.Distributed;

namespace Library.Infrastructure.Data;

public class AlbumCacheInvalidator(IDistributedCache cache) : IAlbumCacheInvalidator
{
    public Task EvictBySlugAsync(string slug, CancellationToken cancellationToken = default) =>
        cache.RemoveAsync($"album:{slug}", cancellationToken);
}
