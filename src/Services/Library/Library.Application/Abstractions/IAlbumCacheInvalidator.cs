namespace Library.Application.Abstractions;

public interface IAlbumCacheInvalidator
{
    Task EvictBySlugAsync(string slug, CancellationToken cancellationToken = default);
}
