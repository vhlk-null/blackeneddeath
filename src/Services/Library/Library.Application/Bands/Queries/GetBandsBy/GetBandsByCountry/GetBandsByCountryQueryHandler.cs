using Library.Application.Bands.Queries.GetBands;
using Library.Infrastructure.Data;

namespace Library.Application.Bands.Queries.GetBandsBy.GetBandsByCountry;

public record GetBandsByCountryQuery(Guid CountryId) : IQuery<GetBandsByCountryResult>;
public record GetBandsByCountryResult(IEnumerable<BandDto> Bands);

internal class GetBandsByCountryQueryHandler(IRepository<LibraryContext> repo)
    : IQueryHandler<GetBandsByCountryQuery, GetBandsByCountryResult>
{
    public async ValueTask<GetBandsByCountryResult> Handle(GetBandsByCountryQuery query, CancellationToken cancellationToken)
    {
        //var bands = await repo.Filter<Band>(b => b.CountryId == query.CountryId)
        //    .Include(b => b.Country)
        //    .Include(b => b.Albums).ThenInclude(ab => ab.Album)
        //    .Include(b => b.Genres).ThenInclude(bg => bg.Genre)
        //    .ProjectToType<BandDto>()
        //    .ToListAsync(cancellationToken);

        return new GetBandsByCountryResult(new BandDto[]{});
    }
}