using System.Linq.Expressions;

namespace Library.Application.Services.Bands.Specifications;

public class BandByGenreSpec(Guid genreId) : Specification<Band>
{
    private readonly GenreId _genreId = GenreId.Of(genreId);

    public override Expression<Func<Band, bool>> Criteria =>
        b => b.BandGenres.Any(bg => bg.GenreId == _genreId);
}
