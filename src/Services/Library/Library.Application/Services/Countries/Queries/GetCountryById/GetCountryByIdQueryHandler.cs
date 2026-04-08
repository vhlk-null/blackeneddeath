namespace Library.Application.Services.Countries.Queries.GetCountryById;

public class GetCountryByIdQueryHandler(ILibraryDbContext context)
    : BuildingBlocks.CQRS.IQueryHandler<GetCountryByIdQuery, GetCountryByIdResult>
{
    public async ValueTask<GetCountryByIdResult> Handle(GetCountryByIdQuery query, CancellationToken cancellationToken)
    {
        CountryId countryId = CountryId.Of(query.Id);

        Country country = await context.Countries
                              .AsNoTracking()
                              .FirstOrDefaultAsync(c => c.Id == countryId, cancellationToken)
                          ?? throw new CountryNotFoundException(query.Id);

        return new GetCountryByIdResult(country.ToCountryDto());
    }
}
