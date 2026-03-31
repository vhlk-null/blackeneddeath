using System.Linq.Expressions;

namespace Library.Application.Services.Albums.Specifications;

public class AlbumByYearSpec(int year) : Specification<Album>
{
    public override Expression<Func<Album, bool>> Criteria =>
        a => a.AlbumRelease.ReleaseYear == year;
}
