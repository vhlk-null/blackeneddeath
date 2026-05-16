namespace Library.Application.Search;

public enum SearchPeriod
{
    AllTime,
    ThisYear,
    ThisMonth
}

public interface IMeilesearchFilter
{
    string ToFilterString();
}

public static class AlbumSearchFilterBuilder
{
    public static string? Build(List<string>? genres, List<string>? countries, string? type, int? releaseYearFrom, int? releaseYearTo, string? labelName = null, double? ratingFrom = null, double? ratingTo = null, bool upcoming = false, SearchPeriod period = SearchPeriod.AllTime, string? sortBy = null, List<string>? excludeTypes = null)
    {
        List<IMeilesearchFilter> filters = new();
        if (genres?.Count > 0) filters.Add(new GenreFilter(genres));
        if (countries?.Count > 0) filters.Add(new CountryFilter(countries));
        if (type is not null) filters.Add(new TypeFilter(type));
        else if (excludeTypes?.Count > 0) filters.Add(new ExcludeTypesFilter(excludeTypes));
        if (releaseYearFrom.HasValue) filters.Add(new ReleaseYearFromFilter(releaseYearFrom.Value));
        if (releaseYearTo.HasValue) filters.Add(new ReleaseYearToFilter(releaseYearTo.Value));
        if (labelName is not null) filters.Add(new LabelFilter(labelName));
        if (ratingFrom.HasValue) filters.Add(new RatingFromFilter(ratingFrom.Value));
        else if (sortBy == "averageRating") filters.Add(new RatingFromFilter(0.1));
        if (ratingTo.HasValue) filters.Add(new RatingToFilter(ratingTo.Value));
        if (sortBy == "ratingsCount") filters.Add(new RatingsCountFromFilter(1));
        if (upcoming) filters.Add(new UpcomingFilter(DateTime.UtcNow));
        if (period != SearchPeriod.AllTime) filters.Add(new CreatedAtFromFilter(DateTime.UtcNow, period));

        return filters.Count > 0 ? string.Join(" AND ", filters.Select(f => f.ToFilterString())) : null;
    }
}

public class GenreFilter(List<string> genres) : IMeilesearchFilter
{
    public string ToFilterString() =>
        string.Join(" OR ", genres.Select(g => $"genres = \"{g}\""));
}

public class CountryFilter(List<string> countries) : IMeilesearchFilter
{
    public string ToFilterString() =>
        string.Join(" OR ", countries.Select(c => $"countries = \"{c}\""));
}

public class TypeFilter(string type) : IMeilesearchFilter
{
    public string ToFilterString() => $"type = \"{type}\"";
}

public class ExcludeTypesFilter(List<string> types) : IMeilesearchFilter
{
    public string ToFilterString() =>
        string.Join(" AND ", types.Select(t => $"type != \"{t}\""));
}

public class ReleaseYearFromFilter(int year) : IMeilesearchFilter
{
    public string ToFilterString() => $"releaseYear >= {year}";
}

public class ReleaseYearToFilter(int year) : IMeilesearchFilter
{
    public string ToFilterString() => $"releaseYear <= {year}";
}

public class LabelFilter(string labelName) : IMeilesearchFilter
{
    public string ToFilterString() => $"label = \"{labelName}\"";
}

public class RatingFromFilter(double rating) : IMeilesearchFilter
{
    public string ToFilterString() => $"averageRating >= {rating}";
}

public class RatingToFilter(double rating) : IMeilesearchFilter
{
    public string ToFilterString() => $"averageRating <= {rating}";
}

public class RatingsCountFromFilter(int count) : IMeilesearchFilter
{
    public string ToFilterString() => $"ratingsCount >= {count}";
}

public class UpcomingFilter(DateTime now) : IMeilesearchFilter
{
    public string ToFilterString() =>
        $"(releaseMonth EXISTS AND releaseDay EXISTS AND ((releaseYear > {now.Year}) OR (releaseYear = {now.Year} AND releaseMonth > {now.Month}) OR (releaseYear = {now.Year} AND releaseMonth = {now.Month} AND releaseDay >= {now.Day})))";
}

public class CreatedAtFromFilter(DateTime now, SearchPeriod period) : IMeilesearchFilter
{
    public string ToFilterString()
    {
        DateTime cutoff = period switch
        {
            SearchPeriod.ThisMonth => new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc),
            SearchPeriod.ThisYear => new DateTime(now.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            _ => DateTime.MinValue
        };
        long unixCutoff = new DateTimeOffset(cutoff).ToUnixTimeSeconds();
        return $"createdAt >= {unixCutoff}";
    }
}