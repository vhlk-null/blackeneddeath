using System.Linq.Expressions;

namespace Library.Application.Services.Bands.Specifications;

// Includes bands whose activity overlaps with [yearFrom, yearTo].
// A band is included if it was formed before/during yearTo
// and disbanded after/during yearFrom (or is still active).
public class BandByYearRangeSpec(int? yearFrom, int? yearTo) : Specification<Band>
{
    public override Expression<Func<Band, bool>> Criteria =>
        b => (!yearFrom.HasValue || !b.Activity.DisbandedYear.HasValue || b.Activity.DisbandedYear.Value >= yearFrom.Value)
          && (!yearTo.HasValue   || !b.Activity.FormedYear.HasValue    || b.Activity.FormedYear.Value   <= yearTo.Value);
}
