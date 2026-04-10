using System.Linq.Expressions;

namespace Library.Application.Services.Albums.Specifications;

public class AlbumByNameSpec(string name) : Specification<Album>
{
    public override Expression<Func<Album, bool>> Criteria =>
        a => a.Title.ToLower().Contains(name.ToLower());
}
