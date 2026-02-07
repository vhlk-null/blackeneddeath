using Mediator;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Behaviors;

public sealed class UnitOfWorkBehavior<TRequest, TResponse>(DbContext context)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull, IMessage
        where TResponse : notnull
{
    public async ValueTask<TResponse> Handle(TRequest message, MessageHandlerDelegate<TRequest, TResponse> next, CancellationToken cancellationToken)
    {        
        var messageTypeName = typeof(TRequest).Name;

        if (messageTypeName.Contains("Query", StringComparison.OrdinalIgnoreCase))
        {
            return await next(message, cancellationToken);
        }

        var response = await next(message, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return response;
    }
}