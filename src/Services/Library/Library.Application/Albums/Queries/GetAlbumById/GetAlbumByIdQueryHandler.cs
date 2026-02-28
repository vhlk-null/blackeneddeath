namespace Library.Application.Albums.Queries.GetAlbumById;

public class GetAlbumByIdQueryHandler(ILibraryDbContext context) : BuildingBlocks.CQRS.IQueryHandler<GetAlbumByIdQuery, GetAlbumByIdResult>
{
    public async ValueTask<GetAlbumByIdResult> Handle(GetAlbumByIdQuery query, CancellationToken cancellationToken)
    {
        var albumId = AlbumId.Of(query.Id);

        var album = await context.Albums
            .AsNoTracking()
            .Include(a => a.AlbumBands)
            .Include(a => a.AlbumGenres)
            .Include(a => a.AlbumCountries)
            .Include(a => a.AlbumTracks)
            .Include(a => a.StreamingLinks)
            .FirstOrDefaultAsync(a => a.Id == albumId, cancellationToken)
            ?? throw new AlbumNotFoundException(query.Id);

        var albumDto = ProjectToAlbumDto(album);

        return new GetAlbumByIdResult(albumDto);
    }
}