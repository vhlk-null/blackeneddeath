using Library.API.Albums.GetAlbums;
using Library.Infrastructure.Data;

namespace Library.API.Albums.GetAlbumsBy.GetAlbumById;

public record GetAlbumByIdQuery(Guid Id) : IQuery<GetAlbumByIdResult>;
public record GetAlbumByIdResult(AlbumDto Album);

internal class GetAlbumByIdQueryHandler(IRepository<LibraryContext> repo)
    : IQueryHandler<GetAlbumByIdQuery, GetAlbumByIdResult>
{
    public async ValueTask<GetAlbumByIdResult> Handle(GetAlbumByIdQuery query, CancellationToken cancellationToken)
    {
        //var album = await repo.GetWithIncludesAsync<Album>(
        //                a => a.Id == query.Id,
        //                q => q.Include(a => a.Bands).ThenInclude(ab => ab.Band)
        //                    .Include(a => a.Countries).ThenInclude(ac => ac.Country)
        //                    .Include(a => a.StreamingLinks)
        //                    .Include(a => a.Tracks).ThenInclude(at => at.Track)
        //                    .Include(a => a.Genres).ThenInclude(ag => ag.Genre),
        //                cancellationToken)
        //            ?? throw new AlbumNotFoundException(query.Id);

        return new GetAlbumByIdResult(null);
    }
}