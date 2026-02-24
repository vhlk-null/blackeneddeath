namespace Library.Domain.ValueObjects;

public record LabelInfo
{
    public string Name { get; init; } = null!;

    private LabelInfo(string name)
    {
        Name = name;
    }

    public static LabelInfo Of(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return new LabelInfo(name);
    }
}
