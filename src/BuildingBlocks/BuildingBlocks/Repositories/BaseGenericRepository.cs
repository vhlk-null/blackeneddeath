using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BuildingBlocks.Repositories
{
    public abstract class BaseGenericRepository<TContext> : IRepository<TContext> where TContext : DbContext
    {
        public required TContext Context { get; set; }

        public async Task<T?> GetByAsync<T>(Expression<Func<T, bool>> expression, bool asTracked = true, CancellationToken cancellationToken = default) where T : class
        {
            return asTracked
                ? await Context.Set<T>().FirstOrDefaultAsync(expression, cancellationToken)
                : await Context.Set<T>().AsNoTracking().FirstOrDefaultAsync(expression, cancellationToken);
        }

        public async Task<T?> GetByWithIncludeAsync<T>(Expression<Func<T, bool>> expression, Expression<Func<T, object>> includeExpression, CancellationToken cancellationToken = default) where T : class
        {
            return await Context.Set<T>()
                .Include(includeExpression)
                .FirstOrDefaultAsync(expression, cancellationToken);
        }

        public async Task<T?> GetWithIncludesAsync<T>(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includes) where T : class
        {
            IQueryable<T> query = Context.Set<T>().AsNoTracking();

            foreach (var include in includes)
                query = query.Include(include);

            return await query.FirstOrDefaultAsync(filter, cancellationToken);
        }

        public async Task<T?> GetWithIncludesAsync<T>(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IQueryable<T>> includeBuilder, CancellationToken cancellationToken = default) where T : class
        {
            var query = includeBuilder(Context.Set<T>().AsNoTracking());
            return await query.FirstOrDefaultAsync(filter, cancellationToken);
        }

        public async Task<List<T>> FilterAsync<T>(Expression<Func<T, bool>> expression, bool asTracked = true, CancellationToken cancellationToken = default) where T : class
        {
            return asTracked
                ? await Context.Set<T>().Where(expression).ToListAsync(cancellationToken)
                : await Context.Set<T>().AsNoTracking().Where(expression).ToListAsync(cancellationToken);
        }

        public IQueryable<T> Filter<T>(Expression<Func<T, bool>> expression, bool asTracked = true) where T : class
        {
            return asTracked
                ? this.Context.Set<T>().Where(expression)
                : this.Context.Set<T>().AsNoTracking().Where(expression);
        }

        public async Task<List<T>> AllAsync<T>(CancellationToken cancellationToken = default) where T : class
        {
            return await Context.Set<T>().ToListAsync(cancellationToken);
        }

        public IQueryable<T> All<T>() where T : class
        {
            return Context.Set<T>().AsQueryable();
        }

        public async Task<List<T>> AllWithIncludeAsync<T>(List<Expression<Func<T, object>>> includeExpressions, CancellationToken cancellationToken = default) where T : class
        {
            IQueryable<T> set = Context.Set<T>();

            foreach (var include in includeExpressions)
            {
                set = set.Include(include);
            }

            return await set.ToListAsync(cancellationToken);
        }

        public async Task AddAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class
        {
            await Context.Set<T>().AddAsync(entity, cancellationToken);
        }

        public async Task AddRangeAsync<T>(IEnumerable<T> entities, CancellationToken cancellationToken = default) where T : class
        {
            await Context.Set<T>().AddRangeAsync(entities, cancellationToken);
        }

        public void Update<T>(T entity) where T : class
        {
            Context.Set<T>().Update(entity);
        }

        public void UpdateRange<T>(IEnumerable<T> entities) where T : class
        {
            Context.Set<T>().UpdateRange(entities);
        }

        public void Delete<T>(T entity) where T : class
        {
            if (Context.Entry(entity).State == EntityState.Detached)
            {
                Context.Set<T>().Attach(entity);
            }

            Context.Set<T>().Remove(entity);
        }

        public void DeleteRange<T>(IEnumerable<T> entities) where T : class
        {
            var enumerableEntities = entities as IList<T> ?? entities.ToList();

            foreach (var entity in enumerableEntities)
            {
                if (Context.Entry(entity).State == EntityState.Detached)
                {
                    Context.Set<T>().Attach(entity);
                }
            }

            Context.Set<T>().RemoveRange(enumerableEntities);
        }

        public async Task DeleteAsync<T>(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default) where T : class
        {
            var entities = await Context.Set<T>().Where(expression).ToListAsync(cancellationToken);
            DeleteRange(entities);
        }

        public async Task<int> CountAsync<T>(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default) where T : class
        {
            return await Context.Set<T>().CountAsync(expression, cancellationToken);
        }

        public async Task<int> CountAsync<T>(CancellationToken cancellationToken = default) where T : class
        {
            return await Context.Set<T>().CountAsync(cancellationToken);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await Context.SaveChangesAsync(cancellationToken);
        }
    }
}