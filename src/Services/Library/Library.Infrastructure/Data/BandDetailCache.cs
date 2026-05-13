using Library.Application.Abstractions;
using Library.Application.Dtos;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Library.Infrastructure.Data;

public class BandDetailCache(
    IDistributedCache cache,
    IConnectionMultiplexer redis,
    ILogger<BandDetailCache> logger) : IBandDetailCache
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        ReferenceHandler = ReferenceHandler.IgnoreCycles
    };

    private static readonly DistributedCacheEntryOptions CacheOptions =
        new() { AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(30) };

    private static string DetailKey(string slug) => $"band-detail:{slug}";
    private static string BandIndexKey(Guid bandId) => $"band-detail-index:{bandId}";

    public async Task<BandDetailDto?> GetAsync(string slug, CancellationToken cancellationToken = default)
    {
        try
        {
            string? json = await cache.GetStringAsync(DetailKey(slug), cancellationToken);
            if (json is not null)
                return JsonSerializer.Deserialize<BandDetailDto>(json, JsonOptions);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Cache GET error [band-detail:{Slug}]", slug);
        }
        return null;
    }

    public async Task SetAsync(string slug, Guid bandId, BandDetailDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            await cache.SetStringAsync(
                DetailKey(slug),
                JsonSerializer.Serialize(dto, JsonOptions),
                CacheOptions,
                cancellationToken);

            await redis.GetDatabase().SetAddAsync(BandIndexKey(bandId), slug);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Cache SET error [band-detail:{Slug}]", slug);
        }
    }

    public async Task InvalidateAsync(Guid bandId, CancellationToken cancellationToken = default)
    {
        try
        {
            IDatabase db = redis.GetDatabase();
            string indexKey = BandIndexKey(bandId);
            RedisValue[] slugs = await db.SetMembersAsync(indexKey);
            foreach (RedisValue slug in slugs)
                await cache.RemoveAsync(DetailKey(slug!), cancellationToken);
            await db.KeyDeleteAsync(indexKey);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Cache invalidation error for band {BandId}", bandId);
        }
    }
}
