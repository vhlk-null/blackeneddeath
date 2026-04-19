namespace Library.Application.Services.Albums.Specifications;

public static class AlbumFilterBuilder
{
    public static ISpecification<Album>? Build(
        List<Guid> genreIds,
        List<Guid> labelIds,
        List<Guid> countryIds,
        List<AlbumType> types,
        int? yearFrom,
        int? yearTo,
        string? name = null)
    {
        ISpecification<Album>? filter = null;

        if (genreIds.Count > 0)   filter = Combine(filter, genreIds.Select(id => (ISpecification<Album>)new AlbumByGenreSpec(id)).Aggregate((a, b) => a.Or(b)));
        if (labelIds.Count > 0)   filter = Combine(filter, labelIds.Select(id => (ISpecification<Album>)new AlbumByLabelSpec(id)).Aggregate((a, b) => a.Or(b)));
        if (countryIds.Count > 0) filter = Combine(filter, countryIds.Select(id => (ISpecification<Album>)new AlbumByCountrySpec(id)).Aggregate((a, b) => a.Or(b)));
        if (types.Count > 0)                     filter = Combine(filter, new AlbumByTypeSpec(types));
        if (yearFrom.HasValue || yearTo.HasValue) filter = Combine(filter, new AlbumByYearRangeSpec(yearFrom, yearTo));
        if (!string.IsNullOrWhiteSpace(name))    filter = Combine(filter, new AlbumByNameSpec(name));

        return filter;
    }

    public static async Task<ISpecification<Album>?> BuildByNameAsync(
        ILibraryDbContext context,
        List<string> genreNames,
        List<string> labelNames,
        List<string> countryNames,
        List<AlbumType> types,
        int? yearFrom,
        int? yearTo,
        string? name = null,
        CancellationToken cancellationToken = default)
    {
        List<Guid> resolvedGenreIds = [];
        List<Guid> resolvedLabelIds = [];
        List<Guid> resolvedCountryIds = [];

        if (genreNames.Count > 0)
        {
            List<string> lower = genreNames.Select(n => n.ToLower()).ToList();
            resolvedGenreIds = await context.Genres.AsNoTracking()
                .Where(g => lower.Contains(g.Name.ToLower()))
                .Select(g => g.Id.Value)
                .ToListAsync(cancellationToken);
        }

        if (labelNames.Count > 0)
        {
            List<string> lower = labelNames.Select(n => n.ToLower()).ToList();
            resolvedLabelIds = await context.Labels.AsNoTracking()
                .Where(l => lower.Contains(l.Name.ToLower()))
                .Select(l => l.Id.Value)
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

        return Build(resolvedGenreIds, resolvedLabelIds, resolvedCountryIds, types, yearFrom, yearTo, name);
    }

    private static ISpecification<Album> Combine(ISpecification<Album>? current, ISpecification<Album> next) =>
        current is null ? next : current.And(next);
}
