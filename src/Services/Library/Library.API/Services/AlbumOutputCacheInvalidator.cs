using Library.Application.Abstractions;
using Library.API.Constants;
using Microsoft.AspNetCore.OutputCaching;

namespace Library.API.Services;

public class AlbumOutputCacheInvalidator(IOutputCacheStore store) : IAlbumCacheInvalidator
{
    public Task EvictBySlugAsync(string slug, CancellationToken cancellationToken = default) =>
        store.EvictByTagAsync(OutputCacheTags.Album(slug), cancellationToken).AsTask();
}
