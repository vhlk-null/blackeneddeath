using System.Linq.Expressions;

namespace Library.Application.Services.Bands.Specifications;

public class BandByFormedYearSpec(int year) : Specification<Band>
{
    public override Expression<Func<Band, bool>> Criteria =>
        b => b.Activity.FormedYear == year;
}
