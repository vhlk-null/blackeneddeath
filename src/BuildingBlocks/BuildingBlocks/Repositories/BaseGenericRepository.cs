using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;

namespace BuildingBlocks.Repositories
{
    public abstract class BaseGenericRepository<TContext> : IRepository<TContext> where TContext : DbContext
    {
        public required TContext Context { get; set; }

        public T? GetBy<T>(Expression<Func<T, bool>> expression, bool asTracked = true) where T : class
        {
            return asTracked
                ? this.Context.Set<T>().FirstOrDefault(expression)
                : this.Context.Set<T>().AsNoTracking().FirstOrDefault(expression);
        }

        public T? GetByWithInclude<T>(Expression<Func<T, bool>> expression, Expression<Func<T, object>> includeExpression) where T : class
        {
            return this.Context.Set<T>().Include(includeExpression).FirstOrDefault(expression);
        }

        public IQueryable<T> Filter<T>(Expression<Func<T, bool>> expression, bool asTracked = true) where T : class
        {
            return asTracked
                ? this.Context.Set<T>().Where(expression)
                : this.Context.Set<T>().AsNoTracking().Where(expression);
        }

        public IQueryable<T> All<T>() where T : class
        {
            return this.Context.Set<T>();
        }

        public IQueryable<T> AllWithInclude<T>(List<Expression<Func<T, object>>> includeExpressions) where T : class
        {
            IQueryable<T> set = this.Context.Set<T>();

            foreach (var include in includeExpressions)
            {
                set = set.Include(include);
            }

            return set;
        }

        public void Add<T>(T entity) where T : class
        {
            this.Context.Set<T>().Add(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            this.Context.Set<T>().Update(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            if (this.Context.Entry(entity).State == EntityState.Detached)
            {
                this.Context.Set<T>().Attach(entity);
            }

            this.Context.Set<T>().Remove(entity);
        }

        public void Delete<T>(IEnumerable<T> entities) where T : class
        {
            var enumerableEntities = entities as IList<T> ?? entities.ToList();

            foreach (var entity in enumerableEntities)
            {
                if (this.Context.Entry(entity).State == EntityState.Detached)
                {
                    this.Context.Set<T>().Attach(entity);
                }
            }

            this.Context.Set<T>().RemoveRange(enumerableEntities);
        }

        public void Delete<T>(Expression<Func<T, bool>> expression) where T : class
        {
            var entities = this.Context.Set<T>().Where(expression).AsEnumerable();

            this.Delete(entities);
        }

        public int Count<T>(Expression<Func<T, bool>> expression) where T : class
        {
            return this.Context.Set<T>().Count(expression);
        }

        public int Count<T>() where T : class
        {
            return this.Context.Set<T>().Count();
        }

        public void SaveChanges()
        {
            this.Context.SaveChanges();
        }
    }
}
