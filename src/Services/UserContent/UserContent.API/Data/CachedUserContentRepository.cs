namespace UserContent.API.Data;

public class CachedUserContentRepository : IRepository<UserContentContext>
{
    private readonly IRepository<UserContentContext> _innerRepository;
    private readonly IDistributedCache _cache;
    private readonly IConnectionMultiplexer _redis;
    private static readonly TimeSpan DefaultCacheDuration = TimeSpan.FromMinutes(30);

    public CachedUserContentRepository(
        IRepository<UserContentContext> innerRepository,
        IDistributedCache cache,
        IConnectionMultiplexer redis)
    {
        _innerRepository = innerRepository;
        _cache = cache;
        _redis = redis;
    }

    public UserContentContext Context
    {
        get => _innerRepository.Context;
        set => _innerRepository.Context = value;
    }

    public async Task<T?> GetWithIncludesAsync<T>(
        Expression<Func<T, bool>> filter,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] includes) where T : class
    {
        var cacheKey = GenerateCacheKey<T>(nameof(GetWithIncludesAsync), filter, includes);

        var cached = await _cache.GetStringAsync(cacheKey, cancellationToken);
        if (cached != null)
            return JsonSerializer.Deserialize<T>(cached);

        var result = await _innerRepository.GetWithIncludesAsync(filter, cancellationToken, includes);

        if (result != null)
        {
            await _cache.SetStringAsync(
                cacheKey, JsonSerializer.Serialize(result), 
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = DefaultCacheDuration},
                cancellationToken);
        }

        return result;
    }

    public async Task<T?> GetByAsync<T>(
        Expression<Func<T, bool>> expression,
        bool asTracked = true,
        CancellationToken cancellationToken = default) where T : class
    {
        if (asTracked)
            return await _innerRepository.GetByAsync(expression, asTracked, cancellationToken);

        var cacheKey = GenerateCacheKey<T>(nameof(GetByAsync), expression);

        var cached = await _cache.GetStringAsync(cacheKey, cancellationToken);
        if (cached != null)
            return JsonSerializer.Deserialize<T>(cached);

        var result = await _innerRepository.GetByAsync(expression, asTracked, cancellationToken);

        if (result != null)
        {
            await _cache.SetStringAsync(
                cacheKey, JsonSerializer.Serialize(result),
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = DefaultCacheDuration },
                cancellationToken);
        }

        return result;
    }

    public async Task<List<T>> FilterAsync<T>(
        Expression<Func<T, bool>> expression,
        bool asTracked = true,
        CancellationToken cancellationToken = default) where T : class
    {
        if (asTracked)
            return await _innerRepository.FilterAsync(expression, asTracked, cancellationToken);

        var cacheKey = GenerateCacheKey<T>(nameof(FilterAsync), expression);

        var cached = await _cache.GetStringAsync(cacheKey, cancellationToken);
        if (cached != null)
            return JsonSerializer.Deserialize<List<T>>(cached)!;

        var result = await _innerRepository.FilterAsync(expression, asTracked, cancellationToken);

        if (result.Any())
        {
            await _cache.SetStringAsync(
                cacheKey, JsonSerializer.Serialize(result),
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = DefaultCacheDuration },
                cancellationToken);
        }

        return result;
    }
   
    public async Task AddAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class
    {
        await _innerRepository.AddAsync(entity, cancellationToken);
        await InvalidateCacheForType<T>();
    }

    public void Update<T>(T entity) where T : class
    {
        _innerRepository.Update(entity);
        InvalidateCacheForType<T>().GetAwaiter().GetResult();
    }

    public void Delete<T>(T entity) where T : class
    {
        _innerRepository.Delete(entity);
        InvalidateCacheForType<T>().GetAwaiter().GetResult();
    }

    private async Task InvalidateCacheForType<T>() where T : class
    {
        var pattern = $"UserContent:{typeof(T).Name}:*";

        try
        {
            var db = _redis.GetDatabase();
            var endpoint = _redis.GetEndPoints().FirstOrDefault();

            if (endpoint != null)
            {
                var server = _redis.GetServer(endpoint);

                await foreach (var key in server.KeysAsync(pattern: pattern))
                {
                    await db.KeyDeleteAsync(key);
                }
            }
        }
        catch { }
    }

    private static string GenerateCacheKey<T>(
        string operation,
        Expression filter,
        params Expression[] includes) where T : class
    {
        var typeName = typeof(T).Name;
        var filterString = filter.ToString();
        var includesString = string.Join(",", includes.Select(i => i.ToString()));
        var hash = $"{filterString}:{includesString}".GetHashCode();
        return $"UserContent:{typeName}:{operation}:{hash}";
    }

    public Task<T?> GetByWithIncludeAsync<T>(
        Expression<Func<T, bool>> expression,
        Expression<Func<T, object>> includeExpression,
        CancellationToken cancellationToken = default) where T : class
        => _innerRepository.GetByWithIncludeAsync(expression, includeExpression, cancellationToken);

    public IQueryable<T> Filter<T>(Expression<Func<T, bool>> expression, bool asTracked = true) where T : class
        => _innerRepository.Filter(expression, asTracked);

    public Task<List<T>> AllAsync<T>(CancellationToken cancellationToken = default) where T : class
        => _innerRepository.AllAsync<T>(cancellationToken);

    public IQueryable<T> All<T>() where T : class
        => _innerRepository.All<T>();

    public Task<List<T>> AllWithIncludeAsync<T>(
        List<Expression<Func<T, object>>> includeExpressions,
        CancellationToken cancellationToken = default) where T : class
        => _innerRepository.AllWithIncludeAsync(includeExpressions, cancellationToken);

    public Task AddRangeAsync<T>(IEnumerable<T> entities, CancellationToken cancellationToken = default) where T : class
        => _innerRepository.AddRangeAsync(entities, cancellationToken);

    public void UpdateRange<T>(IEnumerable<T> entities) where T : class
        => _innerRepository.UpdateRange(entities);

    public void DeleteRange<T>(IEnumerable<T> entities) where T : class
        => _innerRepository.DeleteRange(entities);

    public Task DeleteAsync<T>(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default) where T : class
        => _innerRepository.DeleteAsync(expression, cancellationToken);

    public Task<int> CountAsync<T>(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default) where T : class
        => _innerRepository.CountAsync(expression, cancellationToken);

    public Task<int> CountAsync<T>(CancellationToken cancellationToken = default) where T : class
        => _innerRepository.CountAsync<T>(cancellationToken);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => _innerRepository.SaveChangesAsync(cancellationToken);
}