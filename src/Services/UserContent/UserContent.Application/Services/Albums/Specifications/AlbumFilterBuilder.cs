using BuildingBlocks.Specifications;

namespace UserContent.Application.Services.Albums.Specifications;

public static class AlbumFilterBuilder
{
    public static ISpecification<Album>? Build(
        List<AlbumType> types,
        int? yearFrom,
        int? yearTo,
        string? genreName = null,
        string? name = null)
    {
        ISpecification<Album>? filter = null;

        if (types.Count > 0)                      filter = Combine(filter, new AlbumByTypeSpec(types));
        if (yearFrom.HasValue || yearTo.HasValue)  filter = Combine(filter, new AlbumByYearRangeSpec(yearFrom, yearTo));
        if (!string.IsNullOrWhiteSpace(genreName)) filter = Combine(filter, new AlbumByGenreNameSpec(genreName));
        if (!string.IsNullOrWhiteSpace(name))      filter = Combine(filter, new AlbumByNameSpec(name));

        return filter;
    }

    private static ISpecification<Album> Combine(ISpecification<Album>? current, ISpecification<Album> next) =>
        current is null ? next : current.And(next);
}
