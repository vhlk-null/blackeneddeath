namespace Library.Application.Services.Albums.Queries.GetPendingApprovalAlbums;

public class GetPendingApprovalAlbumsQueryHandler(ILibraryDbContext context)
    : BuildingBlocks.CQRS.IQueryHandler<GetPendingApprovalAlbumsQuery, GetPendingApprovalAlbumsResult>
{
    public async ValueTask<GetPendingApprovalAlbumsResult> Handle(
        GetPendingApprovalAlbumsQuery query,
        CancellationToken cancellationToken)
    {
        List<Album> albums = await context.Albums
            .AsNoTracking()
            .Where(a => !a.IsApproved)
            .Include(a => a.AlbumBands)
            .ToListAsync(cancellationToken);

        List<BandId> bandIds = albums
            .SelectMany(a => a.AlbumBands.Select(ab => ab.BandId))
            .Distinct()
            .ToList();

        Dictionary<BandId, Band> bands = await context.Bands
            .AsNoTracking()
            .Where(b => bandIds.Contains(b.Id))
            .ToDictionaryAsync(b => b.Id, cancellationToken);

        List<PendingApprovalBandGroup> groups = albums
            .SelectMany(a => a.AlbumBands, (a, ab) => new { Album = a, ab.BandId })
            .GroupBy(x => x.BandId)
            .Select(g =>
            {
                Band? band = bands.GetValueOrDefault(g.Key);
                return new PendingApprovalBandGroup(
                    BandId: g.Key.Value,
                    BandName: band?.Name ?? "Unknown",
                    BandSlug: band?.Slug,
                    Albums: g.Select(x => new PendingApprovalDto(
                        x.Album.Id.Value, x.Album.Title, x.Album.Slug, x.Album.CreatedBy)).ToList());
            })
            .OrderBy(g => g.BandName)
            .ToList();

        return new GetPendingApprovalAlbumsResult(groups);
    }
}
