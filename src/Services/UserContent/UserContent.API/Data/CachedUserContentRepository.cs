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
        var userId = ExtractUserIdFromExpression(filter);
        var cacheKey = GenerateCacheKey<T>(nameof(GetWithIncludesAsync), userId, filter, includes);

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

        var userId = ExtractUserIdFromExpression(expression);
        var cacheKey = GenerateCacheKey<T>(nameof(GetByAsync), userId, expression);

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

        var userId = ExtractUserIdFromExpression(expression);
        var cacheKey = GenerateCacheKey<T>(nameof(FilterAsync), userId, expression);

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
        var userId = ExtractUserIdFromEntity(entity);
        await InvalidateCacheForType<T>(userId);
    }

    public void Update<T>(T entity) where T : class
    {
        _innerRepository.Update(entity);
        var userId = ExtractUserIdFromEntity(entity);
        InvalidateCacheForType<T>(userId).GetAwaiter().GetResult();
    }

    public void Delete<T>(T entity) where T : class
    {
        _innerRepository.Delete(entity);
        var userId = ExtractUserIdFromEntity(entity);
        InvalidateCacheForType<T>(userId).GetAwaiter().GetResult();
    }

    private async Task InvalidateCacheForType<T>(Guid? userId = null) where T : class
    {
        var pattern = userId.HasValue
            ? $"UserContent:{userId.Value}:*"
            : $"UserContent:*:*";

        var server = _redis.GetServers().First();
        var keys = server.Keys(pattern: pattern).ToArray();

        if (keys.Length > 0)
        {
            var db = _redis.GetDatabase();
            await db.KeyDeleteAsync(keys);
        }
    }

    private static Guid? ExtractUserIdFromExpression(Expression expression)
    {
        if (expression is LambdaExpression lambda)
            return ExtractUserIdFromExpression(lambda.Body);

        if (expression is BinaryExpression binary)
        {
            if (binary.NodeType is ExpressionType.AndAlso or ExpressionType.OrElse)
                return ExtractUserIdFromExpression(binary.Left)
                    ?? ExtractUserIdFromExpression(binary.Right);

            if (binary.NodeType == ExpressionType.Equal)
            {
                if (IsUserIdMember(binary.Left) && TryGetGuidValue(binary.Right, out var id))
                    return id;
                if (IsUserIdMember(binary.Right) && TryGetGuidValue(binary.Left, out id))
                    return id;
            }
        }

        return null;
    }

    private static bool IsUserIdMember(Expression expression)
        => expression is MemberExpression member && member.Member.Name == "UserId";

    private static bool TryGetGuidValue(Expression expression, out Guid value)
    {
        try
        {
            var lambda = Expression.Lambda<Func<Guid>>(Expression.Convert(expression, typeof(Guid)));
            value = lambda.Compile()();
            return true;
        }
        catch
        {
            value = Guid.Empty;
            return false;
        }
    }

    private static Guid? ExtractUserIdFromEntity<T>(T entity) where T : class
    {
        var prop = typeof(T).GetProperty("UserId");
        if (prop?.PropertyType == typeof(Guid))
            return (Guid)prop.GetValue(entity)!;
        return null;
    }

    private static string GenerateCacheKey<T>(
        string operation,
        Guid? userId,
        Expression filter,
        params Expression[] includes) where T : class
    {
        var typeName = typeof(T).Name;
        var includesString = string.Join(",", includes.Select(i => i.ToString()));
        var rawKey = $"{typeName}:{includesString}";
        var hash = ComputeDeterministicHash(rawKey);
        var userSegment = userId.HasValue ? userId.Value.ToString() : "shared";
        return $"{userSegment}:{typeName}:{operation}:{hash}";
    }

    private static string ComputeDeterministicHash(string input)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(bytes)[..16];
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

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var result = await _innerRepository.SaveChangesAsync(cancellationToken);

        return result;
    }   
}