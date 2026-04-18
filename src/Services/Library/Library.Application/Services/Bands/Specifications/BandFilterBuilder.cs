namespace Library.Application.Services.Bands.Specifications;

public static class BandFilterBuilder
{
    public static ISpecification<Band>? Build(
        Guid? genreId,
        Guid? countryId,
        BandStatus? status,
        int? yearFrom,
        int? yearTo,
        string? name = null)
    {
        ISpecification<Band>? filter = null;

        if (genreId.HasValue)                    filter = Combine(filter, new BandByGenreSpec(genreId.Value));
        if (countryId.HasValue)                  filter = Combine(filter, new BandByCountrySpec(countryId.Value));
        if (status.HasValue)                     filter = Combine(filter, new BandByStatusSpec(status.Value));
        if (yearFrom.HasValue || yearTo.HasValue) filter = Combine(filter, new BandByYearRangeSpec(yearFrom, yearTo));
        if (!string.IsNullOrWhiteSpace(name))    filter = Combine(filter, new BandByNameSpec(name));

        return filter;
    }

    public static async Task<ISpecification<Band>?> BuildBySlugAsync(
        ILibraryDbContext context,
        string? genreSlug,
        string? countryId,
        BandStatus? status,
        int? yearFrom,
        int? yearTo,
        string? name = null,
        CancellationToken cancellationToken = default)
    {
        Guid? resolvedGenreId = null;
        Guid? resolvedCountryId = null;

        if (!string.IsNullOrWhiteSpace(genreSlug))
        {
            Genre? genre = await context.Genres.AsNoTracking()
                .FirstOrDefaultAsync(g => g.Slug == genreSlug, cancellationToken);
            if (genre is not null)
                resolvedGenreId = genre.Id.Value;
        }

        if (!string.IsNullOrWhiteSpace(countryId) && Guid.TryParse(countryId, out Guid parsedCountryId))
            resolvedCountryId = parsedCountryId;

        return Build(resolvedGenreId, resolvedCountryId, status, yearFrom, yearTo, name);
    }

    public static async Task<ISpecification<Band>?> BuildByNameAsync(
        ILibraryDbContext context,
        string? genreName,
        string? countryName,
        BandStatus? status,
        int? yearFrom,
        int? yearTo,
        string? name = null,
        CancellationToken cancellationToken = default)
    {
        Guid? resolvedGenreId = null;
        Guid? resolvedCountryId = null;

        if (!string.IsNullOrWhiteSpace(genreName))
        {
            Genre? genre = await context.Genres.AsNoTracking()
                .FirstOrDefaultAsync(g => g.Name.ToLower() == genreName.ToLower(), cancellationToken);
            if (genre is not null)
                resolvedGenreId = genre.Id.Value;
        }

        if (!string.IsNullOrWhiteSpace(countryName))
        {
            Country? country = await context.Countries.AsNoTracking()
                .FirstOrDefaultAsync(c => c.Name.ToLower() == countryName.ToLower(), cancellationToken);
            if (country is not null)
                resolvedCountryId = country.Id.Value;
        }

        return Build(resolvedGenreId, resolvedCountryId, status, yearFrom, yearTo, name);
    }

    private static ISpecification<Band> Combine(ISpecification<Band>? current, ISpecification<Band> next) =>
        current is null ? next : current.And(next);
}
