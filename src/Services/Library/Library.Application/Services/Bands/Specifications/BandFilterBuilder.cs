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

    private static ISpecification<Band> Combine(ISpecification<Band>? current, ISpecification<Band> next) =>
        current is null ? next : current.And(next);
}
