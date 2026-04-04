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
            var leftExpr  = left.Criteria;
            var rightCriteria = right.Criteria;
            var rightExpr = ParameterReplacer.Replace(rightCriteria, rightCriteria.Parameters[0], leftExpr.Parameters[0]);
            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(leftExpr.Body, rightExpr.Body), leftExpr.Parameters[0]);
        }
    }
}

file sealed class OrSpecification<T>(ISpecification<T> left, ISpecification<T> right) : Specification<T>
{
    public override Expression<Func<T, bool>> Criteria
    {
        get
        {
            var leftExpr  = left.Criteria;
            var rightCriteria = right.Criteria;
            var rightExpr = ParameterReplacer.Replace(rightCriteria, rightCriteria.Parameters[0], leftExpr.Parameters[0]);
            return Expression.Lambda<Func<T, bool>>(Expression.OrElse(leftExpr.Body, rightExpr.Body), leftExpr.Parameters[0]);
        }
    }
}

file sealed class NotSpecification<T>(ISpecification<T> inner) : Specification<T>
{
    public override Expression<Func<T, bool>> Criteria
    {
        get
        {
            var expr = inner.Criteria;
            return Expression.Lambda<Func<T, bool>>(Expression.Not(expr.Body), expr.Parameters[0]);
        }
    }
}

file sealed class ParameterReplacer(ParameterExpression from, ParameterExpression to) : ExpressionVisitor
{
    public static Expression<Func<T, bool>> Replace<T>(
        Expression<Func<T, bool>> expr,
        ParameterExpression from,
        ParameterExpression to) =>
        (Expression<Func<T, bool>>)new ParameterReplacer(from, to).Visit(expr)!;

    protected override Expression VisitParameter(ParameterExpression node) =>
        node == from ? to : base.VisitParameter(node);
}
