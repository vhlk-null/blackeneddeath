using System.Linq.Expressions;

namespace Library.Application.Services.Albums.Specifications;

public class AlbumByYearRangeSpec(int? yearFrom, int? yearTo) : Specification<Album>
{
    public override Expression<Func<Album, bool>> Criteria =>
        a => (!yearFrom.HasValue || a.AlbumRelease.ReleaseYear >= yearFrom.Value)
          && (!yearTo.HasValue   || a.AlbumRelease.ReleaseYear <= yearTo.Value);
}
