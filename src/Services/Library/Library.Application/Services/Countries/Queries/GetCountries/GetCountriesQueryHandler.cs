namespace Library.Application.Services.Countries.Queries.GetCountries;

public class GetCountriesQueryHandler(ILibraryDbContext context)
    : BuildingBlocks.CQRS.IQueryHandler<GetCountriesQuery, GetCountriesResult>
{
    public async ValueTask<GetCountriesResult> Handle(GetCountriesQuery query, CancellationToken cancellationToken)
    {
        List<Country> countries = await context.Countries
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);

        List<CountryDto> countryDtos = countries.Select(c => c.ToCountryDto()).ToList();

        return new GetCountriesResult(countryDtos);
    }
}
