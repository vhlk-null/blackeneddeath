using Library.Application.Abstractions;
using Library.Application.Dtos;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Library.Infrastructure.Data;

public class AlbumDetailCache(
    IDistributedCache cache,
    IConnectionMultiplexer redis,
    ILogger<AlbumDetailCache> logger) : IAlbumDetailCache
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        ReferenceHandler = ReferenceHandler.IgnoreCycles
    };

    private static readonly DistributedCacheEntryOptions CacheOptions =
        new() { AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(30) };

    private static string DetailKey(string slug) => $"album-detail:{slug}";
    private static string BandIndexKey(Guid bandId) => $"band-albums-index:{bandId}";

    public async Task<AlbumDetailDto?> GetAsync(string slug, CancellationToken cancellationToken = default)
    {
        try
        {
            string? json = await cache.GetStringAsync(DetailKey(slug), cancellationToken);
            if (json is not null)
                return JsonSerializer.Deserialize<AlbumDetailDto>(json, JsonOptions);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Cache GET error [album-detail:{Slug}]", slug);
        }
        return null;
    }

    public async Task SetAsync(string slug, IReadOnlyList<Guid> bandIds, AlbumDetailDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            await cache.SetStringAsync(
                DetailKey(slug),
                JsonSerializer.Serialize(dto, JsonOptions),
                CacheOptions,
                cancellationToken);

            IDatabase db = redis.GetDatabase();
            foreach (Guid bandId in bandIds)
                await db.SetAddAsync(BandIndexKey(bandId), slug);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Cache SET error [album-detail:{Slug}]", slug);
        }
    }

    public async Task InvalidateForBandsAsync(IEnumerable<Guid> bandIds, CancellationToken cancellationToken = default)
    {
        try
        {
            IDatabase db = redis.GetDatabase();
            foreach (Guid bandId in bandIds)
            {
                string indexKey = BandIndexKey(bandId);
                RedisValue[] slugs = await db.SetMembersAsync(indexKey);
                foreach (RedisValue slug in slugs)
                    await cache.RemoveAsync(DetailKey(slug!), cancellationToken);
                await db.KeyDeleteAsync(indexKey);
            }
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Cache invalidation error for bands");
        }
    }

    public async Task InvalidateBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        try
        {
            await cache.RemoveAsync(DetailKey(slug), cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Cache invalidation error [album-detail:{Slug}]", slug);
        }
    }
}
