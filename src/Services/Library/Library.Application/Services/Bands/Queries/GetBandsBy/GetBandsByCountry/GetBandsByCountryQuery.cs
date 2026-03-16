namespace Library.Application.Services.Bands.Queries.GetBandsBy.GetBandsByCountry;

public record GetBandsByCountryQuery(Guid CountryId) : BuildingBlocks.CQRS.IQuery<GetBandsByCountryResult>;
public record GetBandsByCountryResult(IEnumerable<BandDto> Bands);
