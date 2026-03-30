namespace Library.Application.Extensions;

public static class GenreCardExtensions
{
    public static GenreCardDto ToGenreCardDto(this GenreCard card, IStorageUrlResolver urlResolver) => new(
        card.Id.Value,
        card.Name,
        card.Description,
        urlResolver.Resolve(card.CoverUrl),
        card.OrderNumber);


    public static GenreCardDetailDto ToGenreCardDetailsDto(
        this GenreCard card,
        IStorageUrlResolver urlResolver,
        Dictionary<TagId, Tag> tagMap,
        Dictionary<GenreId, Genre> genreMap) =>
        new GenreCardDetailDto(
            card.Id.Value,
            card.Name,
            card.Description,
            card.GenreCardTags.Select(t => tagMap[t.TagId]).Select(t => new TagDto(t.Id.Value, t.Name)).ToList(),
            card.GenreCardGenres.Select(g => genreMap[g.GenreId]).Select(g => new GenreDto(g.Id.Value, g.Name, g.Slug, g.ParentGenreId is null)).ToList(),
            urlResolver.Resolve(card.CoverUrl),
            card.OrderNumber);
}
