using System.Linq.Expressions;
using BuildingBlocks.Specifications;

namespace UserContent.Application.Services.Bands.Specifications;

public class BandByGenreNameSpec(string genreName) : Specification<Band>
{
    public override Expression<Func<Band, bool>> Criteria =>
        b => b.PrimaryGenreName != null && b.PrimaryGenreName.ToLower().Contains(genreName.ToLower());
}
