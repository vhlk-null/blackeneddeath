namespace UserContent.API.Data
{
    public class CachedUserContentRepository : IRepository<UserContentContext>
    {
        private readonly IRepository<UserContentContext> _innerRepository;
        private readonly IDistributedCache _cache;
        private static readonly TimeSpan DefaultCacheDuration = TimeSpan.FromMinutes(30);

        public CachedUserContentRepository(IRepository<UserContentContext> innerRepository, IDistributedCache cache)
        {
            _innerRepository = innerRepository;
            _cache = cache;
        }
        public UserContentContext Context
        {
            get => _innerRepository.Context;
            set => _innerRepository.Context = value;
        }

        public async Task<T?> GetWithIncludesAsync<T>(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default,
            params Expression<Func<T, object>>[] includes) where T : class
        {
            var cacheKey = GenerateCacheKey<T>("GetWithIncludes", filter, includes);

            var cached = await _cache.GetStringAsync(cacheKey, cancellationToken);
            if (cached != null)
            {
                return JsonSerializer.Deserialize<T>(cached);
            }

            var result = await _innerRepository.GetWithIncludesAsync(filter, cancellationToken, includes);

            if (result != null)
            {
                await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(result), new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = DefaultCacheDuration
                },
                cancellationToken);
            }

            return result;
        }

        public Task<T?> GetByAsync<T>(
        Expression<Func<T, bool>> expression,
        bool asTracked = true,
        CancellationToken cancellationToken = default) where T : class
        => _innerRepository.GetByAsync(expression, asTracked, cancellationToken);

        public Task<T?> GetByWithIncludeAsync<T>(
            Expression<Func<T, bool>> expression,
            Expression<Func<T, object>> includeExpression,
            CancellationToken cancellationToken = default) where T : class
            => _innerRepository.GetByWithIncludeAsync(expression, includeExpression, cancellationToken);

        public Task<List<T>> FilterAsync<T>(
            Expression<Func<T, bool>> expression,
            bool asTracked = true,
            CancellationToken cancellationToken = default) where T : class
            => _innerRepository.FilterAsync(expression, asTracked, cancellationToken);

        public IQueryable<T> Filter<T>(
            Expression<Func<T, bool>> expression,
            bool asTracked = true) where T : class
            => _innerRepository.Filter(expression, asTracked);

        public IQueryable<T> All<T>() where T : class
            => _innerRepository.All<T>();

        public Task<List<T>> AllAsync<T>(CancellationToken cancellationToken = default) where T : class
            => _innerRepository.AllAsync<T>(cancellationToken);

        public Task<List<T>> AllWithIncludeAsync<T>(
            List<Expression<Func<T, object>>> includeExpressions,
            CancellationToken cancellationToken = default) where T : class
            => _innerRepository.AllWithIncludeAsync(includeExpressions, cancellationToken);

        private static string GenerateCacheKey<T>(string operation, Expression filter, params Expression[] includes) where T : class
        {
            var typeName = typeof(T).Name;
            var filterString = filter.ToString();
            var includesString = string.Join(",", includes.Select(i => i.ToString()));
            var hash = $"{filterString}:{includesString}".GetHashCode();
            return $"UserContent:{typeName}:{operation}:{hash}";
        }

        public Task AddAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class
        {
            throw new NotImplementedException();
        }

        public Task AddRangeAsync<T>(IEnumerable<T> entities, CancellationToken cancellationToken = default) where T : class
        {
            throw new NotImplementedException();
        }

        public void Update<T>(T entity) where T : class
        {
            throw new NotImplementedException();
        }

        public void UpdateRange<T>(IEnumerable<T> entities) where T : class
        {
            throw new NotImplementedException();
        }

        public void Delete<T>(T entity) where T : class
        {
            throw new NotImplementedException();
        }

        public void DeleteRange<T>(IEnumerable<T> entities) where T : class
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync<T>(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<int> CountAsync<T>(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<int> CountAsync<T>(CancellationToken cancellationToken = default) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
