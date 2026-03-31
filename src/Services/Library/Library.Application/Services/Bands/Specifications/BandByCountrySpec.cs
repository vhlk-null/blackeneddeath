using System.Linq.Expressions;

namespace Library.Application.Services.Bands.Specifications;

public class BandByCountrySpec(Guid countryId) : Specification<Band>
{
    private readonly CountryId _countryId = CountryId.Of(countryId);

    public override Expression<Func<Band, bool>> Criteria =>
        b => b.BandCountries.Any(bc => bc.CountryId == _countryId);
}
