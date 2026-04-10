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

    private static ISpecification<Album> Combine(ISpecification<Album>? current, ISpecification<Album> next) =>
        current is null ? next : current.And(next);
}
