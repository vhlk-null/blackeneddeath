using System.Linq.Expressions;
using BuildingBlocks.Specifications;

namespace UserContent.Application.Services.Albums.Specifications;

public class AlbumByGenreNameSpec(string genreName) : Specification<Album>
{
    public override Expression<Func<Album, bool>> Criteria =>
        a => a.PrimaryGenreName != null && a.PrimaryGenreName.ToLower().Contains(genreName.ToLower());
}
