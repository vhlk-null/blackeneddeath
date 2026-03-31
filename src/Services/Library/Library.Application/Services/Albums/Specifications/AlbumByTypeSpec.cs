using System.Linq.Expressions;

namespace Library.Application.Services.Albums.Specifications;

public class AlbumByTypeSpec(AlbumType type) : Specification<Album>
{
    public override Expression<Func<Album, bool>> Criteria =>
        a => a.Type == type;
}
