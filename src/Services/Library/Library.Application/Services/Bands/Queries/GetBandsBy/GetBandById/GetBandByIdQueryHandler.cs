namespace Library.Application.Services.Bands.Queries.GetBandsBy.GetBandById;

public class GetBandByIdQueryHandler(ILibraryDbContext context)
    : BuildingBlocks.CQRS.IQueryHandler<GetBandByIdQuery, GetBandByIdResult>
{
    public async ValueTask<GetBandByIdResult> Handle(GetBandByIdQuery query, CancellationToken cancellationToken)
    {
        var bandId = BandId.Of(query.Id);

        var band = await context.Bands
            .AsNoTracking()
            .Include(b => b.BandCountries)
            .Include(b => b.BandGenres)
            .FirstOrDefaultAsync(b => b.Id == bandId, cancellationToken)
            ?? throw new BandNotFoundException(query.Id);

        var albumIds = await context.AlbumBands.AsNoTracking()
            .Where(ab => ab.BandId == band.Id)
            .Select(ab => ab.AlbumId)
            .ToListAsync(cancellationToken);

        var countryIds = band.BandCountries.Select(bc => bc.CountryId).ToList();
        var genreIds = band.BandGenres.Select(bg => bg.GenreId).ToList();

        var albums = await context.Albums.AsNoTracking()
            .Where(a => albumIds.Contains(a.Id))
            .ToListAsync(cancellationToken);

        var countries = await context.Countries.AsNoTracking()
            .Where(c => countryIds.Contains(c.Id))
            .ToDictionaryAsync(c => c.Id, cancellationToken);

        var genres = await context.Genres.AsNoTracking()
            .Where(g => genreIds.Contains(g.Id))
            .ToDictionaryAsync(g => g.Id, cancellationToken);

        var albumsByBand = albums.ToLookup(_ => band.Id);

        return new GetBandByIdResult(band.ToBandDto(countries, genres, albumsByBand));
    }
}
