namespace Library.Application.Services.Albums.Queries.GetPendingApprovalAlbums;

public class GetPendingApprovalAlbumsQueryHandler(ILibraryDbContext context)
    : BuildingBlocks.CQRS.IQueryHandler<GetPendingApprovalAlbumsQuery, GetPendingApprovalAlbumsResult>
{
    public async ValueTask<GetPendingApprovalAlbumsResult> Handle(
        GetPendingApprovalAlbumsQuery query,
        CancellationToken cancellationToken)
    {
        List<PendingApprovalDto> albums = await context.Albums
            .AsNoTracking()
            .Where(a => !a.IsApproved)
            .Select(a => new PendingApprovalDto(a.Id.Value, a.Title, a.Slug))
            .ToListAsync(cancellationToken);

        return new GetPendingApprovalAlbumsResult(albums);
    }
}
