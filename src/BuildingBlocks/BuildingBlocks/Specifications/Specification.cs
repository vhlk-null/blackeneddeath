using System.Linq.Expressions;

namespace BuildingBlocks.Specifications;

public abstract class Specification<T> : ISpecification<T>
{
    public abstract Expression<Func<T, bool>> Criteria { get; }

    public ISpecification<T> And(ISpecification<T> other) =>
        new AndSpecification<T>(this, other);

    public ISpecification<T> Or(ISpecification<T> other) =>
        new OrSpecification<T>(this, other);

    public ISpecification<T> Not() =>
        new NotSpecification<T>(this);
}

file sealed class AndSpecification<T>(ISpecification<T> left, ISpecification<T> right) : Specification<T>
{
    public override Expression<Func<T, bool>> Criteria
    {
        get
        {
            var param = Expression.Parameter(typeof(T));
            var body = Expression.AndAlso(
                Expression.Invoke(left.Criteria, param),
                Expression.Invoke(right.Criteria, param));
            return Expression.Lambda<Func<T, bool>>(body, param);
        }
    }
}

file sealed class OrSpecification<T>(ISpecification<T> left, ISpecification<T> right) : Specification<T>
{
    public override Expression<Func<T, bool>> Criteria
    {
        get
        {
            var param = Expression.Parameter(typeof(T));
            var body = Expression.OrElse(
                Expression.Invoke(left.Criteria, param),
                Expression.Invoke(right.Criteria, param));
            return Expression.Lambda<Func<T, bool>>(body, param);
        }
    }
}

file sealed class NotSpecification<T>(ISpecification<T> inner) : Specification<T>
{
    public override Expression<Func<T, bool>> Criteria
    {
        get
        {
            var param = Expression.Parameter(typeof(T));
            var body = Expression.Not(Expression.Invoke(inner.Criteria, param));
            return Expression.Lambda<Func<T, bool>>(body, param);
        }
    }
}
