using System.Linq.Expressions;

namespace BuildingBlocks.Repositories
{
    public interface IRepository<TContext> where TContext : class
    {
        TContext Context { get; set; }

        T? GetBy<T>(Expression<Func<T, bool>> expression, bool asTracked = true) where T : class;

        T? GetByWithInclude<T>(Expression<Func<T, bool>> expression, Expression<Func<T, object>> includeExpression) where T : class;

        IQueryable<T> Filter<T>(Expression<Func<T, bool>> expression, bool asTracked = true) where T : class;

        IQueryable<T> All<T>() where T : class;

        IQueryable<T> AllWithInclude<T>(List<Expression<Func<T, object>>> includeExpressions) where T : class;

        void Add<T>(T entity) where T : class;

        void Delete<T>(T entity) where T : class;

        void Delete<T>(IEnumerable<T> entities) where T : class;

        void Delete<T>(Expression<Func<T, bool>> expression) where T : class;

        void Update<T>(T entity) where T : class;

        int Count<T>(Expression<Func<T, bool>> expression) where T : class;

        int Count<T>() where T : class;

        void SaveChanges();
    }
}
