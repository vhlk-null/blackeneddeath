namespace Library.Application.Services.Bands.Queries.GetPendingApprovalVideoBands;

public class GetPendingApprovalVideoBandsQueryHandler(ILibraryDbContext context)
    : BuildingBlocks.CQRS.IQueryHandler<GetPendingApprovalVideoBandsQuery, GetPendingApprovalVideoBandsResult>
{
    public async ValueTask<GetPendingApprovalVideoBandsResult> Handle(
        GetPendingApprovalVideoBandsQuery query,
        CancellationToken cancellationToken)
    {
        List<PendingApprovalDto> videoBands = await context.VideoBands
            .AsNoTracking()
            .Where(vb => !vb.IsApproved)
            .Select(vb => new PendingApprovalDto(vb.Id.Value, vb.Name, null, vb.CreatedBy))
            .ToListAsync(cancellationToken);

        return new GetPendingApprovalVideoBandsResult(videoBands);
    }
}
