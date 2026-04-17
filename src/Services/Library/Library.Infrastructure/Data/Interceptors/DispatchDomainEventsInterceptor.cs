namespace Library.Infrastructure.Data.Interceptors;

internal class DispatchDomainEventsInterceptor(IMediator mediator) : SaveChangesInterceptor
{
    private List<IDomainEvent> _domainEvents = [];

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        CollectDomainEvents(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        CollectDomainEvents(eventData.Context);
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        DispatchDomainEvents().GetAwaiter().GetResult();
        return base.SavedChanges(eventData, result);
    }

    public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        await DispatchDomainEvents();
        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    private void CollectDomainEvents(DbContext? context)
    {
        if (context == null) return;

        IEnumerable<IAggregate> aggregates = context.ChangeTracker.Entries<IAggregate>()
            .Where(a => a.Entity.DomainEvents.Any())
            .Select(a => a.Entity);

        _domainEvents = aggregates.SelectMany(a => a.DomainEvents).ToList();
        aggregates.ToList().ForEach(a => a.ClearDomainEvents());
    }

    private async Task DispatchDomainEvents()
    {
        foreach (IDomainEvent domainEvent in _domainEvents)
            await mediator.Publish(domainEvent);

        _domainEvents = [];
    }
}