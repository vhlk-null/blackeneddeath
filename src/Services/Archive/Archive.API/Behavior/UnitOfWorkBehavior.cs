namespace BuildingBlocks.Behaviors
{
    public class UnitOfWorkBehavior<TRequest, TResponse>(ArchiveContext repo)
    : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
        where TResponse : notnull
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (request is IQuery<TResponse> query)
                return await next();

            var response = await next();

            await repo.SaveChangesAsync(cancellationToken);

            return response;
        }
    }
}
