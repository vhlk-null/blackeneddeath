namespace Library.Application.Services.Countries.Queries.GetCountryById;

public record GetCountryByIdQuery(Guid Id) : BuildingBlocks.CQRS.IQuery<GetCountryByIdResult>;

public record GetCountryByIdResult(CountryDto Country);
