namespace Library.Application.Services.Bands.Queries.GetPendingApprovalBands;

public class GetPendingApprovalBandsQueryHandler(ILibraryDbContext context)
    : BuildingBlocks.CQRS.IQueryHandler<GetPendingApprovalBandsQuery, GetPendingApprovalBandsResult>
{
    public async ValueTask<GetPendingApprovalBandsResult> Handle(
        GetPendingApprovalBandsQuery query,
        CancellationToken cancellationToken)
    {
        List<PendingApprovalDto> bands = await context.Bands
            .AsNoTracking()
            .Where(b => !b.IsApproved)
            .Select(b => new PendingApprovalDto(b.Id.Value, b.Name, b.Slug, b.CreatedBy))
            .ToListAsync(cancellationToken);

        return new GetPendingApprovalBandsResult(bands);
    }
}
