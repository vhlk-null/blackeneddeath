using System.Linq.Expressions;

namespace Library.Application.Services.Bands.Specifications;

public class BandByNameSpec(string name) : Specification<Band>
{
    public override Expression<Func<Band, bool>> Criteria =>
        b => b.Name.ToLower().Contains(name.ToLower());
}
