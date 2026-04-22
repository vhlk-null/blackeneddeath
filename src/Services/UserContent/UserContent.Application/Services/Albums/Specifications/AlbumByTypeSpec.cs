using System.Linq.Expressions;
using BuildingBlocks.Specifications;

namespace UserContent.Application.Services.Albums.Specifications;

public class AlbumByTypeSpec(List<AlbumType> types) : Specification<Album>
{
    private readonly List<int> _typeInts = types.Select(t => (int)t).ToList();

    public override Expression<Func<Album, bool>> Criteria =>
        a => _typeInts.Contains(a.Type);
}
