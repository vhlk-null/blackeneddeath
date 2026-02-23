namespace Library.Domain.ValueObjects;

public record LabelInfo
{
    public string Name { get; init; }

    private LabelInfo() { }
}
