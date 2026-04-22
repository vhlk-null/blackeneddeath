using System.Linq.Expressions;
using BuildingBlocks.Specifications;

namespace UserContent.Application.Services.Bands.Specifications;

public class BandByNameSpec(string name) : Specification<Band>
{
    public override Expression<Func<Band, bool>> Criteria =>
        b => b.BandName.ToLower().Contains(name.ToLower());
}
