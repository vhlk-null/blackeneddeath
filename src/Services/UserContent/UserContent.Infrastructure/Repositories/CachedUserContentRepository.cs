using Microsoft.Extensions.Logging;

namespace UserContent.Infrastructure.Repositories;

public class CachedUserContentRepository(
    IUserContentRepository innerRepository,
    IDistributedCache cache,
    IConnectionMultiplexer redis,
    ILogger<CachedUserContentRepository> logger)
    : IUserContentRepository
{
    private static readonly TimeSpan DefaultCacheDuration = TimeSpan.FromMinutes(30);
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        ReferenceHandler = ReferenceHandler.IgnoreCycles
    };

    public async Task<UserProfileInfo?> GetUserProfileWithDetailsAsync(Guid userId, CancellationToken ct = default)
    {
        var cacheKey = $"{userId}:UserProfileInfo:GetUserProfileWithDetailsAsync";

        try
        {
            var cached = await cache.GetStringAsync(cacheKey, ct);
            if (cached != null)
                return JsonSerializer.Deserialize<UserProfileInfo>(cached, JsonOptions);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Cache read failed for key {CacheKey}", cacheKey);
        }

        var result = await innerRepository.GetUserProfileWithDetailsAsync(userId, ct);

        if (result != null)
        {
            try
            {
                await cache.SetStringAsync(
                    cacheKey,
                    JsonSerializer.Serialize(result, JsonOptions),
                    new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = DefaultCacheDuration },
                    ct);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Cache write failed for key {CacheKey}", cacheKey);
            }
        }

        return result;
    }

    public async Task<Album?> GetAlbumAsync(Guid albumId, CancellationToken ct = default)
    {
        var cacheKey = $"{albumId}:Album:GetAlbumAsync";

        try
        {
            var cached = await cache.GetStringAsync(cacheKey, ct);
            if (cached != null)
                return JsonSerializer.Deserialize<Album>(cached, JsonOptions);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Cache read failed for key {CacheKey}", cacheKey);
        }

        var result = await innerRepository.GetAlbumAsync(albumId, ct);

        if (result != null)
        {
            try
            {
                await cache.SetStringAsync(
                    cacheKey,
                    JsonSerializer.Serialize(result, JsonOptions),
                    new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = DefaultCacheDuration },
                    ct);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Cache write failed for key {CacheKey}", cacheKey);
            }
        }

        return result;
    }

    public Task<FavoriteAlbum?> GetFavoriteAlbumAsync(Guid userId, Guid albumId, CancellationToken ct = default)
        => innerRepository.GetFavoriteAlbumAsync(userId, albumId, ct);

    public Task<FavoriteBand?> GetFavoriteBandAsync(Guid userId, Guid bandId, CancellationToken ct = default)
        => innerRepository.GetFavoriteBandAsync(userId, bandId, ct);

    public async Task AddAsync<T>(T entity, CancellationToken ct = default) where T : class
    {
        await innerRepository.AddAsync(entity, ct);
        var userId = ExtractUserIdFromEntity(entity);
        await InvalidateCacheForUserAsync(userId);
    }

    public void Remove<T>(T entity) where T : class
    {
        innerRepository.Remove(entity);
        var userId = ExtractUserIdFromEntity(entity);
        InvalidateCacheForUser(userId);
    }

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => innerRepository.SaveChangesAsync(ct);

    private async Task InvalidateCacheForUserAsync(Guid? userId)
    {
        var pattern = userId.HasValue
            ? $"UserContent:{userId.Value}:*"
            : "UserContent:*";

        try
        {
            var server = redis.GetServers().First();
            var keys = server.Keys(pattern: pattern).ToArray();

            if (keys.Length > 0)
            {
                var db = redis.GetDatabase();
                await db.KeyDeleteAsync(keys);
            }
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Cache invalidation failed for pattern {Pattern}", pattern);
        }
    }

    private void InvalidateCacheForUser(Guid? userId)
    {
        var pattern = userId.HasValue
            ? $"UserContent:{userId.Value}:*"
            : "UserContent:*";

        try
        {
            var server = redis.GetServers().First();
            var keys = server.Keys(pattern: pattern).ToArray();

            if (keys.Length > 0)
            {
                var db = redis.GetDatabase();
                db.KeyDelete(keys);
            }
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Cache invalidation failed for pattern {Pattern}", pattern);
        }
    }

    private static Guid? ExtractUserIdFromEntity<T>(T? entity) where T : class
    {
        if (entity is null)
            return null;

        var prop = typeof(T).GetProperty("UserId");
        if (prop?.PropertyType == typeof(Guid))
            return (Guid)prop.GetValue(entity)!;
        return null;
    }
}
