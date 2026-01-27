using Archive.API.Bands.GetBands;

namespace Archive.API.Bands.GetBandsBy.GetBandById;

public record GetBandByIdQuery(Guid Id) : IQuery<GetBandByIdResult>;
public record GetBandByIdResult(BandDto Band);

internal class GetBandByIdQueryHandler(IRepository<ArchiveContext> repo)
    : IQueryHandler<GetBandByIdQuery, GetBandByIdResult>
{
    public async Task<GetBandByIdResult> Handle(GetBandByIdQuery query, CancellationToken cancellationToken)
    {
        var band = await repo.Filter<Band>(b => b.Id == query.Id)
            .Include(b => b.Country)
            .Include(b => b.Albums).ThenInclude(ab => ab.Album)
            .Include(b => b.Genres).ThenInclude(bg => bg.Genre)
            .ProjectToType<BandDto>()
            .FirstOrDefaultAsync(cancellationToken);

        if (band == null) throw new BandNotFoundException(query.Id);

        return new GetBandByIdResult(band);
    }
}
