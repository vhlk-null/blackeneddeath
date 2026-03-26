namespace Library.Application.Extensions;

public static class TagExtensions
{
    public static TagDto ToTagDto(this Tag tag) => new(tag.Id.Value, tag.Name);
}
