namespace Library.Application.Services.Bands.Queries.GetBandSummaries;

public class GetBandSummariesQueryHandler(ILibraryDbContext context)
    : BuildingBlocks.CQRS.IQueryHandler<GetBandSummariesQuery, GetBandSummariesResult>
{
    public async ValueTask<GetBandSummariesResult> Handle(GetBandSummariesQuery query, CancellationToken cancellationToken)
    {
        var bands = (await context.Bands
            .AsNoTracking()
            .Select(b => new { b.Id, b.Name, b.Slug, b.Status })
            .ToListAsync(cancellationToken))
            .Select(b => new BandSummaryDto(b.Id.Value, b.Name, b.Slug, b.Status, []))
            .ToList();

        return new GetBandSummariesResult(bands);
    }
}
