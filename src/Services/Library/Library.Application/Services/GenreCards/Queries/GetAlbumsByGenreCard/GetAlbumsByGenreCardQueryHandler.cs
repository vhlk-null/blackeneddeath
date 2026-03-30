namespace Library.Application.Services.GenreCards.Queries.GetAlbumsByGenreCard;

public class GetAlbumsByGenreCardQueryHandler(ILibraryDbContext context, IStorageUrlResolver urlResolver)
    : BuildingBlocks.CQRS.IQueryHandler<GetAlbumsByGenreCardQuery, GetAlbumsByGenreCardResult>
{
    public async ValueTask<GetAlbumsByGenreCardResult> Handle(GetAlbumsByGenreCardQuery query, CancellationToken cancellationToken)
    {
        var cardId = GenreCardId.Of(query.GenreCardId);

        var card = await context.GenreCards
            .AsNoTracking()
            .Include(c => c.GenreCardGenres)
            .Include(c => c.GenreCardTags)
            .FirstOrDefaultAsync(c => c.Id == cardId, cancellationToken)
            ?? throw new GenreCardNotFoundException(query.GenreCardId);

        var genreIds = card.GenreCardGenres.Select(g => g.GenreId).ToList();
        var tagIds = card.GenreCardTags.Select(t => t.TagId).ToList();

        var pageIndex = query.PaginationRequest.PageIndex;
        var pageSize = query.PaginationRequest.PageSize;

        var albumsQuery = context.Albums
            .AsNoTracking()
            .Include(a => a.AlbumBands)
            .Include(a => a.AlbumGenres)
            .Include(a => a.AlbumCountries)
            .Include(a => a.AlbumTracks)
            .Include(a => a.AlbumTags)
            .Include(a => a.StreamingLinks)
            .Where(a =>
                a.AlbumGenres.Any(ag => genreIds.Contains(ag.GenreId)) ||
                a.AlbumTags.Any(at => tagIds.Contains(at.TagId)));

        var totalCount = await albumsQuery.LongCountAsync(cancellationToken);

        var albums = await albumsQuery
            .OrderByDescending(a => a.AlbumRelease.ReleaseYear)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var bandIds = albums.SelectMany(a => a.AlbumBands.Select(ab => ab.BandId)).Distinct().ToList();
        var countryIds = albums.SelectMany(a => a.AlbumCountries.Select(ac => ac.CountryId)).Distinct().ToList();
        var trackIds = albums.SelectMany(a => a.AlbumTracks.Select(at => at.TrackId)).Distinct().ToList();
        var labelIds = albums.Where(a => a.LabelId != null).Select(a => a.LabelId!).Distinct().ToList();
        var albumTagIds = albums.SelectMany(a => a.AlbumTags.Select(at => at.TagId)).Distinct().ToList();

        var bands = await context.Bands.AsNoTracking()
            .Where(b => bandIds.Contains(b.Id))
            .ToDictionaryAsync(b => b.Id, cancellationToken);

        var discographyAlbums = await context.Albums.AsNoTracking()
            .Include(a => a.AlbumBands)
            .Include(a => a.AlbumGenres)
            .Where(a => a.AlbumBands.Any(ab => bandIds.Contains(ab.BandId)))
            .ToListAsync(cancellationToken);

        var discographyByBand = discographyAlbums
            .SelectMany(a => a.AlbumBands.Select(ab => (ab.BandId, a)))
            .ToLookup(x => x.BandId, x => x.a);

        var allGenreIds = albums.SelectMany(a => a.AlbumGenres.Select(ag => ag.GenreId))
            .Concat(discographyAlbums.SelectMany(a => a.AlbumGenres.Select(ag => ag.GenreId)))
            .Distinct().ToList();

        var genres = await context.Genres.AsNoTracking()
            .Where(g => allGenreIds.Contains(g.Id))
            .ToDictionaryAsync(g => g.Id, cancellationToken);

        var countries = await context.Countries.AsNoTracking()
            .Where(c => countryIds.Contains(c.Id))
            .ToDictionaryAsync(c => c.Id, cancellationToken);

        var tracks = await context.Tracks.AsNoTracking()
            .Where(t => trackIds.Contains(t.Id))
            .ToDictionaryAsync(t => t.Id, cancellationToken);

        var labels = labelIds.Count > 0
            ? await context.Labels.AsNoTracking()
                .Where(l => labelIds.Contains(l.Id))
                .ToDictionaryAsync(l => l.Id, cancellationToken)
            : new Dictionary<LabelId, Label>();

        var tags = albumTagIds.Count > 0
            ? await context.Tags.AsNoTracking()
                .Where(t => albumTagIds.Contains(t.Id))
                .ToDictionaryAsync(t => t.Id, cancellationToken)
            : new Dictionary<TagId, Tag>();

        var albumDtos = albums
            .Select(a => a.ToAlbumDto(bands, genres, countries, tracks, urlResolver, labels, tags, discographyByBand))
            .ToList();

        return new GetAlbumsByGenreCardResult(new PaginatedResult<AlbumDto>(pageIndex, pageSize, totalCount, albumDtos));
    }
}
