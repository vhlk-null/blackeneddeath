using System.Linq.Expressions;

namespace BuildingBlocks.MongoDB.Repositories;

public interface IMongoRepository<T> where T : class
{
    Task<T?> GetByAsync(Expression<Func<T, bool>> filter, CancellationToken ct = default);
    Task<List<T>> FilterAsync(Expression<Func<T, bool>> filter, CancellationToken ct = default);
    Task<List<T>> AllAsync(CancellationToken ct = default);

    Task AddAsync(T entity, CancellationToken ct = default);
    Task AddRangeAsync(IEnumerable<T> entities, CancellationToken ct = default);

    Task UpdateAsync(Expression<Func<T, bool>> filter, T entity, CancellationToken ct = default);

    Task DeleteAsync(Expression<Func<T, bool>> filter, CancellationToken ct = default);

    Task<long> CountAsync(Expression<Func<T, bool>> filter, CancellationToken ct = default);
    Task<long> CountAsync(CancellationToken ct = default);
}
