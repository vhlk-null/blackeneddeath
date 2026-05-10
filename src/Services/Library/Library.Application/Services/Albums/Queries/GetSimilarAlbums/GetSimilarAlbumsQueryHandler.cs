namespace Library.Application.Services.Albums.Queries.GetSimilarAlbums;

public class GetSimilarAlbumsQueryHandler(ILibraryDbContext context, IStorageUrlResolver urlResolver)
    : BuildingBlocks.CQRS.IQueryHandler<GetSimilarAlbumsQuery, GetSimilarAlbumsResult>
{
    public async ValueTask<GetSimilarAlbumsResult> Handle(GetSimilarAlbumsQuery query, CancellationToken cancellationToken)
    {
        Album? album = await context.Albums
            .AsNoTracking()
            .Include(a => a.AlbumGenres)
            .Include(a => a.AlbumTags)
            .Include(a => a.AlbumBands)
            .FirstOrDefaultAsync(a => a.Slug == query.Slug && (!query.ApprovedOnly || a.IsApproved), cancellationToken)
            ?? throw new AlbumBySlugNotFoundException(query.Slug);

        List<GenreId> genreIds = album.AlbumGenres.Select(ag => ag.GenreId).ToList();
        List<TagId> tagIds = album.AlbumTags.Select(at => at.TagId).ToList();
        HashSet<AlbumId> excludeIds = [album.Id];

        IQueryable<Album> baseFilter = context.Albums.AsNoTracking()
            .Where(a => (!query.ApprovedOnly || a.IsApproved) && !excludeIds.Contains(a.Id))
            .Where(a => a.AlbumGenres.Any(ag => genreIds.Contains(ag.GenreId))
                     || (tagIds.Count > 0 && a.AlbumTags.Any(at => tagIds.Contains(at.TagId))));

        int totalCount = await baseFilter.CountAsync(cancellationToken);

        List<Album> similarAlbums = await baseFilter
            .OrderBy(_ => EF.Functions.Random())
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .Include(a => a.AlbumGenres)
            .Include(a => a.AlbumCountries)
            .Include(a => a.AlbumBands)
            .ToListAsync(cancellationToken);

        List<BandId> bandIds = similarAlbums
            .SelectMany(a => a.AlbumBands.Select(ab => ab.BandId))
            .Distinct().ToList();

        Dictionary<BandId, Band> bands = await context.Bands.AsNoTracking()
            .Where(b => bandIds.Contains(b.Id))
            .ToDictionaryAsync(b => b.Id, cancellationToken);

        List<GenreId> allGenreIds = similarAlbums
            .SelectMany(a => a.AlbumGenres.Select(ag => ag.GenreId))
            .Distinct().ToList();

        Dictionary<GenreId, Genre> genres = (await context.GetAllGenresAsync(cancellationToken))
            .Where(g => allGenreIds.Contains(g.Id))
            .ToDictionary(g => g.Id);

        List<CountryId> allCountryIds = similarAlbums
            .SelectMany(a => a.AlbumCountries.Select(ac => ac.CountryId))
            .Distinct().ToList();

        Dictionary<CountryId, Country> countries = (await context.GetAllCountriesAsync(cancellationToken))
            .Where(c => allCountryIds.Contains(c.Id))
            .ToDictionary(c => c.Id);

        List<AlbumSummaryDto> items = similarAlbums.Select(a =>
        {
            Band? primaryBand = a.AlbumBands
                .Select(ab => bands.GetValueOrDefault(ab.BandId))
                .FirstOrDefault(b => b is not null);
            return new AlbumSummaryDto(
                a.Id.Value, a.Title, a.Slug, a.AlbumRelease.ReleaseYear,
                a.AlbumRelease.ReleaseMonth, a.AlbumRelease.ReleaseDay,
                urlResolver.Resolve(a.CoverUrl), a.Type, a.AlbumRelease.Format,
                a.AlbumGenres
                    .Where(ag => genres.ContainsKey(ag.GenreId))
                    .Select(ag => new GenreDto(genres[ag.GenreId].Id.Value, genres[ag.GenreId].Name, genres[ag.GenreId].Slug, ag.IsPrimary))
                    .ToList(),
                a.AlbumCountries
                    .Where(ac => countries.ContainsKey(ac.CountryId))
                    .Select(ac => new CountryDto(countries[ac.CountryId].Id.Value, countries[ac.CountryId].Name, countries[ac.CountryId].Code))
                    .ToList(),
                primaryBand?.Id.Value ?? Guid.Empty,
                primaryBand?.Name ?? string.Empty,
                a.IsExplicit);
        }).ToList();

        return new GetSimilarAlbumsResult(new PaginatedResult<AlbumSummaryDto>(query.PageNumber, query.PageSize, totalCount, items));
    }
}
