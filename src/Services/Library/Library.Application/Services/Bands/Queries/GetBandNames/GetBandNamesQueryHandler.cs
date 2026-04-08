namespace Library.Application.Services.Bands.Queries.GetBandNames;

public class GetBandNamesQueryHandler(ILibraryDbContext context)
    : BuildingBlocks.CQRS.IQueryHandler<GetBandNamesQuery, GetBandNamesResult>
{
    public async ValueTask<GetBandNamesResult> Handle(GetBandNamesQuery query, CancellationToken cancellationToken)
    {
        List<NameIdDto> bands = await context.Bands
            .AsNoTracking()
            .Select(b => new NameIdDto(b.Id.Value, b.Name))
            .ToListAsync(cancellationToken);

        return new GetBandNamesResult(bands);
    }
}
