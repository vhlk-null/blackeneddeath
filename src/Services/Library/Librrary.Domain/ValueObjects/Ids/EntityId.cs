namespace Library.Domain.ValueObjects.Ids;

public abstract record EntityId<TValue>
{
    public required TValue Value { get; set; }
}
