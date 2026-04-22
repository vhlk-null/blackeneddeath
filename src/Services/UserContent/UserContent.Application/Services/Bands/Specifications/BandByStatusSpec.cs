using System.Linq.Expressions;
using BuildingBlocks.Specifications;

namespace UserContent.Application.Services.Bands.Specifications;

public class BandByStatusSpec(BandStatus status) : Specification<Band>
{
    public override Expression<Func<Band, bool>> Criteria =>
        b => b.Status == (int)status;
}
