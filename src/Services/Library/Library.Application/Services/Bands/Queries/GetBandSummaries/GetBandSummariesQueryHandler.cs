namespace Library.Application.Services.Bands.Queries.GetBandSummaries;

public class GetBandSummariesQueryHandler(ILibraryDbContext context)
    : BuildingBlocks.CQRS.IQueryHandler<GetBandSummariesQuery, GetBandSummariesResult>
{
    public async ValueTask<GetBandSummariesResult> Handle(GetBandSummariesQuery query, CancellationToken cancellationToken)
    {
        var bands = await context.Bands
            .AsNoTracking()
            .Select(b => new BandSummaryDto(b.Id.Value, b.Name, b.Slug))
            .ToListAsync(cancellationToken);

        return new GetBandSummariesResult(bands);
    }
}
