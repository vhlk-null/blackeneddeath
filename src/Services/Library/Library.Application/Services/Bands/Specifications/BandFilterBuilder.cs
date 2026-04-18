namespace Library.Application.Services.Bands.Specifications;

public static class BandFilterBuilder
{
    public static ISpecification<Band>? Build(
        List<Guid> genreIds,
        List<Guid> countryIds,
        BandStatus? status,
        int? yearFrom,
        int? yearTo,
        string? name = null)
    {
        ISpecification<Band>? filter = null;

        if (genreIds.Count > 0)   filter = Combine(filter, genreIds.Select(id => (ISpecification<Band>)new BandByGenreSpec(id)).Aggregate((a, b) => a.Or(b)));
        if (countryIds.Count > 0) filter = Combine(filter, countryIds.Select(id => (ISpecification<Band>)new BandByCountrySpec(id)).Aggregate((a, b) => a.Or(b)));
        if (status.HasValue)                     filter = Combine(filter, new BandByStatusSpec(status.Value));
        if (yearFrom.HasValue || yearTo.HasValue) filter = Combine(filter, new BandByYearRangeSpec(yearFrom, yearTo));
        if (!string.IsNullOrWhiteSpace(name))    filter = Combine(filter, new BandByNameSpec(name));

        return filter;
    }

    public static async Task<ISpecification<Band>?> BuildByNameAsync(
        ILibraryDbContext context,
        List<string> genreNames,
        List<string> countryNames,
        BandStatus? status,
        int? yearFrom,
        int? yearTo,
        string? name = null,
        CancellationToken cancellationToken = default)
    {
        List<Guid> resolvedGenreIds = [];
        List<Guid> resolvedCountryIds = [];

        if (genreNames.Count > 0)
        {
            List<string> lower = genreNames.Select(n => n.ToLower()).ToList();
            resolvedGenreIds = await context.Genres.AsNoTracking()
                .Where(g => lower.Contains(g.Name.ToLower()))
                .Select(g => g.Id.Value)
                .ToListAsync(cancellationToken);
        }

        if (countryNames.Count > 0)
        {
            List<string> lower = countryNames.Select(n => n.ToLower()).ToList();
            resolvedCountryIds = await context.Countries.AsNoTracking()
                .Where(c => lower.Contains(c.Name.ToLower()))
                .Select(c => c.Id.Value)
                .ToListAsync(cancellationToken);
        }

        return Build(resolvedGenreIds, resolvedCountryIds, status, yearFrom, yearTo, name);
    }

    private static ISpecification<Band> Combine(ISpecification<Band>? current, ISpecification<Band> next) =>
        current is null ? next : current.And(next);
}
