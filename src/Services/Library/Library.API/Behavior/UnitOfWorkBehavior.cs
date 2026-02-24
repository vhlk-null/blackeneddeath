using Library.API.Data;

namespace Library.API.Behavior;

public class UnitOfWorkBehavior<TRequest, TResponse>(LibraryContext repo)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IMessage
    where TResponse : notnull
{
    public async ValueTask<TResponse> Handle(TRequest message, MessageHandlerDelegate<TRequest, TResponse> next, CancellationToken cancellationToken)
    {
        if (message is IQuery<TResponse> query)
            return await next(message, cancellationToken);

        var response = await next(message, cancellationToken);

        await repo.SaveChangesAsync(cancellationToken);

        return response;
    }
}