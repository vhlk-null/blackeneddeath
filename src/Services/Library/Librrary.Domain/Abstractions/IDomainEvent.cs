namespace Library.Domain.Abstractions;

public interface IDomainEvent : INotification
{
    Guid EvenId => Guid.NewGuid();

    public DateTime OccuredOn => DateTime.UtcNow;

    public string? EventType => GetType().AssemblyQualifiedName;
}