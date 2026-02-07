using System.Linq.Expressions;

namespace BuildingBlocks.Repositories
{
    public interface IRepository<TContext> where TContext : class
    {
        TContext Context { get; set; }

        Task<T?> GetByAsync<T>(Expression<Func<T, bool>> expression, bool asTracked = true, CancellationToken cancellationToken = default) where T : class;
        Task<T?> GetByWithIncludeAsync<T>(Expression<Func<T, bool>> expression, Expression<Func<T, object>> includeExpression, CancellationToken cancellationToken = default) where T : class;
        Task<T?> GetWithIncludesAsync<T>(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includes) where T : class;
        Task<List<T>> FilterAsync<T>(Expression<Func<T, bool>> expression, bool asTracked = true, CancellationToken cancellationToken = default) where T : class;
        IQueryable<T> Filter<T>(Expression<Func<T, bool>> expression, bool asTracked = true) where T : class;
        Task<List<T>> AllAsync<T>(CancellationToken cancellationToken = default) where T : class;
        IQueryable<T> All<T>() where T : class;
        Task<List<T>> AllWithIncludeAsync<T>(List<Expression<Func<T, object>>> includeExpressions, CancellationToken cancellationToken = default) where T : class;

        Task AddAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class;
        Task AddRangeAsync<T>(IEnumerable<T> entities, CancellationToken cancellationToken = default) where T : class;

        void Update<T>(T entity) where T : class;
        void UpdateRange<T>(IEnumerable<T> entities) where T : class;

        void Delete<T>(T entity) where T : class;
        void DeleteRange<T>(IEnumerable<T> entities) where T : class;
        Task DeleteAsync<T>(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default) where T : class;

        Task<int> CountAsync<T>(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default) where T : class;
        Task<int> CountAsync<T>(CancellationToken cancellationToken = default) where T : class;

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}