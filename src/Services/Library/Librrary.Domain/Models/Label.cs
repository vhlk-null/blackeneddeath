namespace Library.Domain.Models;

public class Label : Entity<LabelId>
{
    public string Name { get; private set; } = null!;

    private Label() { }

    public static Label Create(LabelId id, string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return new Label { Id = id, Name = name };
    }

    public Label Update(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        Name = name;

        return this;
    }
}
