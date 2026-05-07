using System.Linq.Expressions;
using MongoDB.Driver;

namespace BuildingBlocks.MongoDB.Repositories;

public abstract class BaseMongoRepository<T> : IMongoRepository<T> where T : class
{
    protected readonly IMongoCollection<T> Collection;

    protected BaseMongoRepository(IMongoDatabase database, string? collectionName = null)
    {
        Collection = database.GetCollection<T>(collectionName ?? typeof(T).Name);
    }

    public async Task<T?> GetByAsync(Expression<Func<T, bool>> filter, CancellationToken ct = default)
        => await Collection.Find(filter).FirstOrDefaultAsync(ct);

    public async Task<List<T>> FilterAsync(Expression<Func<T, bool>> filter, CancellationToken ct = default)
        => await Collection.Find(filter).ToListAsync(ct);

    public async Task<List<T>> AllAsync(CancellationToken ct = default)
        => await Collection.Find(Builders<T>.Filter.Empty).ToListAsync(ct);

    public async Task AddAsync(T entity, CancellationToken ct = default)
        => await Collection.InsertOneAsync(entity, cancellationToken: ct);

    public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken ct = default)
        => await Collection.InsertManyAsync(entities, cancellationToken: ct);

    public async Task UpdateAsync(Expression<Func<T, bool>> filter, T entity, CancellationToken ct = default)
        => await Collection.ReplaceOneAsync(filter, entity, cancellationToken: ct);

    public async Task DeleteAsync(Expression<Func<T, bool>> filter, CancellationToken ct = default)
        => await Collection.DeleteOneAsync(filter, ct);

    public async Task<long> CountAsync(Expression<Func<T, bool>> filter, CancellationToken ct = default)
        => await Collection.CountDocumentsAsync(filter, cancellationToken: ct);

    public async Task<long> CountAsync(CancellationToken ct = default)
        => await Collection.CountDocumentsAsync(Builders<T>.Filter.Empty, cancellationToken: ct);
}
