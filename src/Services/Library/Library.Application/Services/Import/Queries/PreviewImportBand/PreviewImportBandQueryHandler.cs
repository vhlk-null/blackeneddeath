namespace Library.Application.Services.Import.Queries.PreviewImportBand;

public class PreviewImportBandQueryHandler(
    IMusicBrainzImportService musicBrainz,
    ILibraryDbContext context)
    : BuildingBlocks.CQRS.IQueryHandler<PreviewImportBandQuery, BandPreviewResult>
{
    public async ValueTask<BandPreviewResult> Handle(PreviewImportBandQuery query, CancellationToken cancellationToken)
    {
        HashSet<string> existingSlugs = (await context.Albums
            .Select(a => a.Slug)
            .ToListAsync(cancellationToken))
            .Where(s => s != null)
            .Select(s => s!)
            .ToHashSet();

        return await musicBrainz.PreviewByMbIdAsync(query.MbId, existingSlugs, cancellationToken);
    }
}
