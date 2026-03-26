namespace Library.Application.Services.Countries.Queries.GetCountries;

public record GetCountriesQuery() : BuildingBlocks.CQRS.IQuery<GetCountriesResult>;

public record GetCountriesResult(IReadOnlyList<CountryDto> Countries);
