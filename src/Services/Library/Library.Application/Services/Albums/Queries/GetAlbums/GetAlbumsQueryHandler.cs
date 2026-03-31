namespace Library.Application.Services.Albums.Queries.GetAlbums;

public class GetAlbumsQueryHandler(ILibraryDbContext context, IStorageUrlResolver urlResolver)
    : BuildingBlocks.CQRS.IQueryHandler<GetAlbumsQuery, GetAlbumsResult>
{
    public async ValueTask<GetAlbumsResult> Handle(GetAlbumsQuery query, CancellationToken cancellationToken)
    {
        var pageIndex = query.PaginationRequest.PageIndex;
        var pageSize = query.PaginationRequest.PageSize;

        var filteredQuery = context.Albums.AsNoTracking();

        if (query.Filter is not null)
            filteredQuery = filteredQuery.Where(query.Filter.Criteria);

        var totalCount = await filteredQuery.LongCountAsync(cancellationToken);

        var albumsQuery = filteredQuery
            .Include(a => a.AlbumBands)
            .Include(a => a.AlbumGenres)
            .Include(a => a.AlbumCountries)
            .Include(a => a.AlbumTracks)
            .Include(a => a.AlbumTags)
            .Include(a => a.StreamingLinks);

        var sorted = query.SortBy switch
        {
            AlbumSortBy.Oldest      => albumsQuery.OrderBy(a => a.CreatedAt),
            AlbumSortBy.ReleaseDate => albumsQuery.OrderByDescending(a => a.AlbumRelease.ReleaseYear),
            AlbumSortBy.Title       => albumsQuery.OrderBy(a => a.Title),
            _                       => albumsQuery.OrderByDescending(a => a.CreatedAt)
        };

        var albums = await sorted
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var bandIds    = albums.SelectMany(a => a.AlbumBands.Select(ab => ab.BandId)).Distinct().ToList();
        var countryIds = albums.SelectMany(a => a.AlbumCountries.Select(ac => ac.CountryId)).Distinct().ToList();
        var trackIds   = albums.SelectMany(a => a.AlbumTracks.Select(at => at.TrackId)).Distinct().ToList();
        var labelIds   = albums.Where(a => a.LabelId != null).Select(a => a.LabelId!).Distinct().ToList();
        var tagIds     = albums.SelectMany(a => a.AlbumTags.Select(at => at.TagId)).Distinct().ToList();

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

        var genreIds = albums.SelectMany(a => a.AlbumGenres.Select(ag => ag.GenreId))
            .Concat(discographyAlbums.SelectMany(a => a.AlbumGenres.Select(ag => ag.GenreId)))
            .Distinct().ToList();

        var genres = await context.Genres.AsNoTracking()
            .Where(g => genreIds.Contains(g.Id))
            .ToDictionaryAsync(g => g.Id, cancellationToken);

        var countries = await context.Countries.AsNoTracking()
            .Where(c => countryIds.Contains(c.Id))
            .ToDictionaryAsync(c => c.Id, cancellationToken);

        var tracks = await context.Tracks.AsNoTracking()
            .Where(t => trackIds.Contains(t.Id))
            .ToDictionaryAsync(t => t.Id, cancellationToken);

        var labels = await context.Labels.AsNoTracking()
            .Where(l => labelIds.Contains(l.Id))
            .ToDictionaryAsync(l => l.Id, cancellationToken);

        var tags = tagIds.Count > 0
            ? await context.Tags.AsNoTracking()
                .Where(t => tagIds.Contains(t.Id))
                .ToDictionaryAsync(t => t.Id, cancellationToken)
            : new Dictionary<TagId, Tag>();

        var albumDtos = albums
            .Select(a => a.ToAlbumDto(bands, genres, countries, tracks, urlResolver, labels, tags, discographyByBand))
            .ToList();

        return new GetAlbumsResult(new PaginatedResult<AlbumDto>(pageIndex, pageSize, totalCount, albumDtos));
    }
}
