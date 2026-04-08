namespace Library.Application.Services.GenreCards.Queries.GetGenreDetailsCards;

public class GetGenreDetailsCardsQueryHandler(ILibraryDbContext context, IStorageUrlResolver urlResolver)
    : BuildingBlocks.CQRS.IQueryHandler<GetGenreDetailsCardsQuery, GetGenreDetailsCardsResult>
{
    public async ValueTask<GetGenreDetailsCardsResult> Handle(GetGenreDetailsCardsQuery query, CancellationToken cancellationToken)
    {
        List<GenreCard> cards = await context.GenreCards
            .AsNoTracking()
            .Include(a => a.GenreCardTags)
            .Include(a => a.GenreCardGenres)
            .OrderBy(c => c.OrderNumber)
            .ToListAsync(cancellationToken);

        List<TagId> tagIds = cards.SelectMany(c => c.GenreCardTags.Select(t => t.TagId)).Distinct().ToList();
        List<GenreId> genreIds = cards.SelectMany(c => c.GenreCardGenres.Select(g => g.GenreId)).Distinct().ToList();

        List<Tag> tags = await context.Tags
            .Where(t => tagIds.Contains(t.Id))
            .ToListAsync(cancellationToken);

        List<Genre> genres = await context.Genres
            .Where(g => genreIds.Contains(g.Id))
            .ToListAsync(cancellationToken);

        Dictionary<TagId, Tag> tagMap = tags.ToDictionary(t => t.Id);
        Dictionary<GenreId, Genre> genreMap = genres.ToDictionary(g => g.Id);

        List<GenreCardDetailDto> dtos = cards
            .Select(c => c.ToGenreCardDetailsDto(urlResolver, tagMap, genreMap))
            .ToList();

        return new GetGenreDetailsCardsResult(dtos);
    }
}
