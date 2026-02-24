namespace Library.Domain.ValueObjects.Ids;

public abstract record EntityId<TValue>
{
    public TValue Value { get; set; } = default!;

    public static TId Of<TId>(TValue value) where TId : EntityId<TValue>, new()
    {
        ArgumentNullException.ThrowIfNull(value);

        return new TId { Value = value };
    }
}
