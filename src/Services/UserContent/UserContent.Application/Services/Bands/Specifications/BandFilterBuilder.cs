using BuildingBlocks.Specifications;

namespace UserContent.Application.Services.Bands.Specifications;

public static class BandFilterBuilder
{
    public static ISpecification<Band>? Build(
        BandStatus? status,
        int? yearFrom,
        int? yearTo,
        string? genreName = null,
        string? name = null)
    {
        ISpecification<Band>? filter = null;

        if (status.HasValue)                       filter = Combine(filter, new BandByStatusSpec(status.Value));
        if (yearFrom.HasValue || yearTo.HasValue)  filter = Combine(filter, new BandByYearRangeSpec(yearFrom, yearTo));
        if (!string.IsNullOrWhiteSpace(genreName)) filter = Combine(filter, new BandByGenreNameSpec(genreName));
        if (!string.IsNullOrWhiteSpace(name))      filter = Combine(filter, new BandByNameSpec(name));

        return filter;
    }

    private static ISpecification<Band> Combine(ISpecification<Band>? current, ISpecification<Band> next) =>
        current is null ? next : current.And(next);
}
