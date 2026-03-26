namespace Library.Application.Extensions;

public static class CountryExtensions
{
    public static CountryDto ToCountryDto(this Country country) => new(
        country.Id.Value,
        country.Name,
        country.Code);
}
