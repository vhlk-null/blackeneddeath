using System.Linq.Expressions;
using BuildingBlocks.Specifications;

namespace UserContent.Application.Services.Bands.Specifications;

public class BandByYearRangeSpec(int? yearFrom, int? yearTo) : Specification<Band>
{
    public override Expression<Func<Band, bool>> Criteria =>
        b => (!yearFrom.HasValue || (b.FormedYear.HasValue && b.FormedYear.Value >= yearFrom.Value))
          && (!yearTo.HasValue   || (b.FormedYear.HasValue && b.FormedYear.Value <= yearTo.Value));
}
