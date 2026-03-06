namespace UserContent.Infrastructure.Repositories;

public class CachedUserContentRepository(
    IRepository<UserContentContext> inner,
    IDistributedCache cache,
    IConnectionMultiplexer redis,
    ILogger<CachedUserContentRepository> logger)
    : IRepository<UserContentContext>
{
    private static readonly TimeSpan DefaultCacheDuration = TimeSpan.FromMinutes(30);
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        ReferenceHandler = ReferenceHandler.IgnoreCycles
    };

    public UserContentContext Context
    {
        get => inner.Context;
        set => inner.Context = value;
    }

    public async Task<T?> GetWithIncludesAsync<T>(
        Expression<Func<T, bool>> filter,
        Func<IQueryable<T>, IQueryable<T>> includeBuilder,
        CancellationToken cancellationToken = default) where T : class
    {
        if (typeof(T) != typeof(UserProfileInfo))
            return await inner.GetWithIncludesAsync(filter, includeBuilder, cancellationToken);

        var userId = TryExtractUserIdFromFilter(filter as Expression<Func<UserProfileInfo, bool>>);
        if (!userId.HasValue)
            return await inner.GetWithIncludesAsync(filter, includeBuilder, cancellationToken);

        var cacheKey = $"{userId.Value}:UserProfileInfo";

        try
        {
            var cached = await cache.GetStringAsync(cacheKey, cancellationToken);
            if (cached != null)
                return JsonSerializer.Deserialize<T>(cached, JsonOptions);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Cache read failed for key {CacheKey}", cacheKey);
        }

        var result = await inner.GetWithIncludesAsync(filter, includeBuilder, cancellationToken);

        if (result != null)
        {
            try
            {
                await cache.SetStringAsync(
                    cacheKey,
                    JsonSerializer.Serialize(result, JsonOptions),
                    new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = DefaultCacheDuration },
                    cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Cache write failed for key {CacheKey}", cacheKey);
            }
        }

        return result;
    }

    public async Task AddAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class
    {
        await inner.AddAsync(entity, cancellationToken);
        await InvalidateCacheForUserAsync(ExtractUserIdFromEntity(entity));
    }

    public async Task AddRangeAsync<T>(IEnumerable<T> entities, CancellationToken cancellationToken = default) where T : class
    {
        var list = entities as IList<T> ?? entities.ToList();
        await inner.AddRangeAsync(list, cancellationToken);
        foreach (var entity in list)
            await InvalidateCacheForUserAsync(ExtractUserIdFromEntity(entity));
    }

    public void Delete<T>(T entity) where T : class
    {
        inner.Delete(entity);
        InvalidateCacheForUser(ExtractUserIdFromEntity(entity));
    }

    public void DeleteRange<T>(IEnumerable<T> entities) where T : class
    {
        var list = entities as IList<T> ?? entities.ToList();
        inner.DeleteRange(list);
        foreach (var entity in list)
            InvalidateCacheForUser(ExtractUserIdFromEntity(entity));
    }

    public async Task DeleteAsync<T>(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default) where T : class
    {
        await inner.DeleteAsync(expression, cancellationToken);
        await InvalidateCacheForUserAsync(null);
    }

    // Remaining methods delegate directly — no caching needed

    public Task<T?> GetByAsync<T>(Expression<Func<T, bool>> expression, bool asTracked = true, CancellationToken cancellationToken = default) where T : class
        => inner.GetByAsync(expression, asTracked, cancellationToken);

    public Task<T?> GetByWithIncludeAsync<T>(Expression<Func<T, bool>> expression, Expression<Func<T, object>> includeExpression, CancellationToken cancellationToken = default) where T : class
        => inner.GetByWithIncludeAsync(expression, includeExpression, cancellationToken);

    public Task<T?> GetWithIncludesAsync<T>(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includes) where T : class
        => inner.GetWithIncludesAsync(filter, cancellationToken, includes);

    public Task<List<T>> FilterAsync<T>(Expression<Func<T, bool>> expression, bool asTracked = true, CancellationToken cancellationToken = default) where T : class
        => inner.FilterAsync(expression, asTracked, cancellationToken);

    public IQueryable<T> Filter<T>(Expression<Func<T, bool>> expression, bool asTracked = true) where T : class
        => inner.Filter(expression, asTracked);

    public Task<List<T>> AllAsync<T>(CancellationToken cancellationToken = default) where T : class
        => inner.AllAsync<T>(cancellationToken);

    public IQueryable<T> All<T>() where T : class
        => inner.All<T>();

    public Task<List<T>> AllWithIncludeAsync<T>(List<Expression<Func<T, object>>> includeExpressions, CancellationToken cancellationToken = default) where T : class
        => inner.AllWithIncludeAsync(includeExpressions, cancellationToken);

    public void Update<T>(T entity) where T : class
        => inner.Update(entity);

    public void UpdateRange<T>(IEnumerable<T> entities) where T : class
        => inner.UpdateRange(entities);

    public Task<int> CountAsync<T>(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default) where T : class
        => inner.CountAsync(expression, cancellationToken);

    public Task<int> CountAsync<T>(CancellationToken cancellationToken = default) where T : class
        => inner.CountAsync<T>(cancellationToken);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => inner.SaveChangesAsync(cancellationToken);

    // Helpers

    private static Guid? TryExtractUserIdFromFilter(Expression<Func<UserProfileInfo, bool>>? filter)
    {
        if (filter?.Body is not BinaryExpression { NodeType: ExpressionType.Equal } binary)
            return null;

        return TryGetGuidValue(binary.Left, binary.Right, "UserId")
            ?? TryGetGuidValue(binary.Right, binary.Left, "UserId");
    }

    private static Guid? TryGetGuidValue(Expression memberSide, Expression valueSide, string memberName)
    {
        if (memberSide is not MemberExpression { Member.Name: var name } || name != memberName)
            return null;

        try
        {
            var value = Expression.Lambda(valueSide).Compile().DynamicInvoke();
            return value is Guid g ? g : null;
        }
        catch
        {
            return null;
        }
    }

    private static Guid? ExtractUserIdFromEntity<T>(T? entity) where T : class
    {
        if (entity is null) return null;
        var prop = typeof(T).GetProperty("UserId");
        if (prop?.PropertyType == typeof(Guid))
            return (Guid)prop.GetValue(entity)!;
        return null;
    }

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
                await redis.GetDatabase().KeyDeleteAsync(keys);
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
                redis.GetDatabase().KeyDelete(keys);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Cache invalidation failed for pattern {Pattern}", pattern);
        }
    }
}
