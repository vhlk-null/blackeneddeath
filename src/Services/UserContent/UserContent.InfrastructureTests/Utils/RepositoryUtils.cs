using System.Linq.Expressions;
using BuildingBlocks.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Moq;

namespace UserContent.InfrastructureTests.Utils
{
    internal class RepositoryUtils
    {
        public static void SetupMockRepository<TRepository, TEntity>(
            Mock<IRepository<TRepository>> mockRepository, params TEntity[] entities)
            where TRepository : DbContext
            where TEntity : class
        {
            mockRepository
                .Setup(x => x.All<TEntity>())
                .Returns(entities.AsQueryable());

            mockRepository
                .Setup(x => x.Filter(It.IsAny<Expression<Func<TEntity, bool>>>(), It.IsAny<bool>()))
                .Returns<Expression<Func<TEntity, bool>>, bool>((expression, _) => entities.AsQueryable().Where(expression));

            mockRepository
                .Setup(x => x.GetByAsync<TEntity>(It.IsAny<Expression<Func<TEntity, bool>>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .Returns<Expression<Func<TEntity, bool>>, bool, CancellationToken>((expression, _, _) =>
                    Task.FromResult(entities.AsQueryable().FirstOrDefault(expression)));

            mockRepository
                .Setup(x => x.GetByWithIncludeAsync<TEntity>(It.IsAny<Expression<Func<TEntity, bool>>>(), It.IsAny<Expression<Func<TEntity, object>>>(), It.IsAny<CancellationToken>()))
                .Returns<Expression<Func<TEntity, bool>>, Expression<Func<TEntity, object>>, CancellationToken>((expression, _, _) =>
                    Task.FromResult(entities.AsQueryable().FirstOrDefault(expression)));

            mockRepository.Setup(x => x.Context.Set<TEntity>()).Returns(MockDbSetFactory.Create(entities).Object);
        }
    }

    internal static class MockDbSetFactory
    {
        public static Mock<DbSet<T>> Create<T>(params T[] data) where T : class
        {
            var queryable = data.AsQueryable();
            var mockDbSet = new Mock<DbSet<T>>();

            mockDbSet.As<IQueryable<T>>().Setup(x => x.Provider).Returns(new TestAsyncQueryProvider<T>(queryable.Provider));
            mockDbSet.As<IQueryable<T>>().Setup(x => x.Expression).Returns(queryable.Expression);
            mockDbSet.As<IQueryable<T>>().Setup(x => x.ElementType).Returns(queryable.ElementType);
            mockDbSet.As<IQueryable<T>>().Setup(x => x.GetEnumerator()).Returns(queryable.GetEnumerator());
            mockDbSet.As<IAsyncEnumerable<T>>().Setup(x => x.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestAsyncEnumerator<T>(queryable.GetEnumerator()));

            return mockDbSet;
        }
    }

    internal class TestAsyncQueryProvider<T>(IQueryProvider inner) : IAsyncQueryProvider
    {
        public IQueryable CreateQuery(Expression expression) => inner.CreateQuery(expression);
        public IQueryable<TElement> CreateQuery<TElement>(Expression expression) => new List<TElement>().AsQueryable();
        public object? Execute(Expression expression) => inner.Execute(expression);
        public TResult Execute<TResult>(Expression expression) => inner.Execute<TResult>(expression);
        public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default)
        {
            var result = inner.Execute(expression);
            return (TResult)typeof(Task).GetMethod(nameof(Task.FromResult))!
                .MakeGenericMethod(typeof(TResult).GetGenericArguments()[0])
                .Invoke(null, [result])!;
        }
    }

    internal class TestAsyncEnumerator<T>(IEnumerator<T> inner) : IAsyncEnumerator<T>
    {
        public T Current => inner.Current;
        public ValueTask<bool> MoveNextAsync() => ValueTask.FromResult(inner.MoveNext());
        public ValueTask DisposeAsync() { inner.Dispose(); return ValueTask.CompletedTask; }
    }
}
