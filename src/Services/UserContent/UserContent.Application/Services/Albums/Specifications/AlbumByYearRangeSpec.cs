using System.Linq.Expressions;
using BuildingBlocks.Specifications;

namespace UserContent.Application.Services.Albums.Specifications;

public class AlbumByYearRangeSpec(int? yearFrom, int? yearTo) : Specification<Album>
{
    public override Expression<Func<Album, bool>> Criteria =>
        a => (!yearFrom.HasValue || a.ReleaseDate >= yearFrom.Value)
          && (!yearTo.HasValue   || a.ReleaseDate <= yearTo.Value);
}
