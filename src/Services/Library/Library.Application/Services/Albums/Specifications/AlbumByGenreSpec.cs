using System.Linq.Expressions;

namespace Library.Application.Services.Albums.Specifications;

public class AlbumByGenreSpec(Guid genreId) : Specification<Album>
{
    private readonly GenreId _genreId = GenreId.Of(genreId);

    public override Expression<Func<Album, bool>> Criteria =>
        a => a.AlbumGenres.Any(ag => ag.GenreId == _genreId);
}
