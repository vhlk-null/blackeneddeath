namespace Library.Application.Services.Albums.Specifications;

public static class AlbumFilterBuilder
{
    public static ISpecification<Album>? Build(
        Guid? genreId,
        Guid? labelId,
        Guid? countryId,
        AlbumType? type,
        int? yearFrom,
        int? yearTo,
        string? name = null)
    {
        ISpecification<Album>? filter = null;

        if (genreId.HasValue)                    filter = Combine(filter, new AlbumByGenreSpec(genreId.Value));
        if (labelId.HasValue)                    filter = Combine(filter, new AlbumByLabelSpec(labelId.Value));
        if (countryId.HasValue)                  filter = Combine(filter, new AlbumByCountrySpec(countryId.Value));
        if (type.HasValue)                       filter = Combine(filter, new AlbumByTypeSpec(type.Value));
        if (yearFrom.HasValue || yearTo.HasValue) filter = Combine(filter, new AlbumByYearRangeSpec(yearFrom, yearTo));
        if (!string.IsNullOrWhiteSpace(name))    filter = Combine(filter, new AlbumByNameSpec(name));

        return filter;
    }

    public static async Task<ISpecification<Album>?> BuildBySlugAsync(
        ILibraryDbContext context,
        string? genreSlug,
        string? labelId,
        string? countryId,
        AlbumType? type,
        int? yearFrom,
        int? yearTo,
        string? name = null,
        CancellationToken cancellationToken = default)
    {
        Guid? resolvedGenreId = null;
        Guid? resolvedLabelId = null;
        Guid? resolvedCountryId = null;

        if (!string.IsNullOrWhiteSpace(genreSlug))
        {
            Genre? genre = await context.Genres.AsNoTracking()
                .FirstOrDefaultAsync(g => g.Slug == genreSlug, cancellationToken);
            if (genre is not null)
                resolvedGenreId = genre.Id.Value;
        }

        if (!string.IsNullOrWhiteSpace(labelId) && Guid.TryParse(labelId, out Guid parsedLabelId))
            resolvedLabelId = parsedLabelId;

        if (!string.IsNullOrWhiteSpace(countryId) && Guid.TryParse(countryId, out Guid parsedCountryId))
            resolvedCountryId = parsedCountryId;

        return Build(resolvedGenreId, resolvedLabelId, resolvedCountryId, type, yearFrom, yearTo, name);
    }

    public static async Task<ISpecification<Album>?> BuildByNameAsync(
        ILibraryDbContext context,
        string? genreName,
        string? labelName,
        string? countryName,
        AlbumType? type,
        int? yearFrom,
        int? yearTo,
        string? name = null,
        CancellationToken cancellationToken = default)
    {
        Guid? resolvedGenreId = null;
        Guid? resolvedLabelId = null;
        Guid? resolvedCountryId = null;

        if (!string.IsNullOrWhiteSpace(genreName))
        {
            Genre? genre = await context.Genres.AsNoTracking()
                .FirstOrDefaultAsync(g => g.Name.ToLower() == genreName.ToLower(), cancellationToken);
            if (genre is not null)
                resolvedGenreId = genre.Id.Value;
        }

        if (!string.IsNullOrWhiteSpace(labelName))
        {
            Label? label = await context.Labels.AsNoTracking()
                .FirstOrDefaultAsync(l => l.Name.ToLower() == labelName.ToLower(), cancellationToken);
            if (label is not null)
                resolvedLabelId = label.Id.Value;
        }

        if (!string.IsNullOrWhiteSpace(countryName))
        {
            Country? country = await context.Countries.AsNoTracking()
                .FirstOrDefaultAsync(c => c.Name.ToLower() == countryName.ToLower(), cancellationToken);
            if (country is not null)
                resolvedCountryId = country.Id.Value;
        }

        return Build(resolvedGenreId, resolvedLabelId, resolvedCountryId, type, yearFrom, yearTo, name);
    }

    private static ISpecification<Album> Combine(ISpecification<Album>? current, ISpecification<Album> next) =>
        current is null ? next : current.And(next);
}
