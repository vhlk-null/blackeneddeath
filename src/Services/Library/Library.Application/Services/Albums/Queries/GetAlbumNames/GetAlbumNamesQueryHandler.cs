namespace Library.Application.Services.Albums.Queries.GetAlbumNames;

public class GetAlbumNamesQueryHandler(ILibraryDbContext context)
    : BuildingBlocks.CQRS.IQueryHandler<GetAlbumNamesQuery, GetAlbumNamesResult>
{
    public async ValueTask<GetAlbumNamesResult> Handle(GetAlbumNamesQuery query, CancellationToken cancellationToken)
    {
        List<NameIdDto> albums = await context.Albums
            .AsNoTracking()
            .Select(a => new NameIdDto(a.Id.Value, a.Title))
            .ToListAsync(cancellationToken);

        return new GetAlbumNamesResult(albums);
    }
}
