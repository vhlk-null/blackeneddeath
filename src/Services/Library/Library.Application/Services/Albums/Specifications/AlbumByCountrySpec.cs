using System.Linq.Expressions;

namespace Library.Application.Services.Albums.Specifications;

public class AlbumByCountrySpec(Guid countryId) : Specification<Album>
{
    private readonly CountryId _countryId = CountryId.Of(countryId);

    public override Expression<Func<Album, bool>> Criteria =>
        a => a.AlbumCountries.Any(ac => ac.CountryId == _countryId);
}
