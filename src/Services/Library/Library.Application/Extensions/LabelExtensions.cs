namespace Library.Application.Extensions;

public static class LabelExtensions
{
    public static LabelDto ToLabelDto(this Label label) => new(label.Id.Value, label.Name);
}
