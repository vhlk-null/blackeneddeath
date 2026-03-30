namespace Library.Application.Services.GenreCards.Queries.GetGenreDetailsCards;

public class GetGenreDetailsCardsQueryHandler(ILibraryDbContext context, IStorageUrlResolver urlResolver)
    : BuildingBlocks.CQRS.IQueryHandler<GetGenreDetailsCardsQuery, GetGenreDetailsCardsResult>
{
    public async ValueTask<GetGenreDetailsCardsResult> Handle(GetGenreDetailsCardsQuery query, CancellationToken cancellationToken)
    {
        var cards = await context.GenreCards
            .AsNoTracking()
            .Include(a => a.GenreCardTags)
            .Include(a => a.GenreCardGenres)
            .OrderBy(c => c.OrderNumber)
            .ToListAsync(cancellationToken);

        var tagIds = cards.SelectMany(c => c.GenreCardTags.Select(t => t.TagId)).Distinct().ToList();
        var genreIds = cards.SelectMany(c => c.GenreCardGenres.Select(g => g.GenreId)).Distinct().ToList();

        var tags = await context.Tags
            .Where(t => tagIds.Contains(t.Id))
            .ToListAsync(cancellationToken);

        var genres = await context.Genres
            .Where(g => genreIds.Contains(g.Id))
            .ToListAsync(cancellationToken);

        var tagMap = tags.ToDictionary(t => t.Id);
        var genreMap = genres.ToDictionary(g => g.Id);

        var dtos = cards
            .Select(c => c.ToGenreCardDetailsDto(urlResolver, tagMap, genreMap))
            .ToList();

        return new GetGenreDetailsCardsResult(dtos);
    }
}
