using System.Linq.Expressions;

namespace Library.Application.Services.Albums.Specifications;

public class AlbumByTypeSpec(List<AlbumType> types) : Specification<Album>
{
    public override Expression<Func<Album, bool>> Criteria =>
        a => types.Contains(a.Type);
}
