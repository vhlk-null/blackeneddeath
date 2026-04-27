namespace Library.Application.Search;

public static class BandSearchFilterBuilder
{
    public static string? Build(
        List<string>? genres,
        List<string>? countries,
        string? status,
        int? formedYearFrom,
        int? formedYearTo)
    {
        List<IMeilesearchFilter> filters = [];

        if (genres?.Count > 0) filters.Add(new GenreFilter(genres));
        if (countries?.Count > 0) filters.Add(new CountryFilter(countries));
        if (status is not null) filters.Add(new BandStatusFilter(status));
        if (formedYearFrom.HasValue) filters.Add(new FormedYearFromFilter(formedYearFrom.Value));
        if (formedYearTo.HasValue) filters.Add(new FormedYearToFilter(formedYearTo.Value));

        return filters.Count > 0 ? string.Join(" AND ", filters.Select(f => f.ToFilterString())) : null;
    }
}

public class BandStatusFilter(string status) : IMeilesearchFilter
{
    public string ToFilterString() => $"status = \"{status}\"";
}

public class FormedYearFromFilter(int year) : IMeilesearchFilter
{
    public string ToFilterString() => $"formedYear >= {year}";
}

public class FormedYearToFilter(int year) : IMeilesearchFilter
{
    public string ToFilterString() => $"formedYear <= {year}";
}
