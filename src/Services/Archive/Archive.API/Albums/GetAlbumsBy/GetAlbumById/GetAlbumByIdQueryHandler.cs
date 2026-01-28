using Archive.API.Albums.GetAlbums;
using BuildingBlocks.Models;

namespace Archive.API.Albums.GetAlbumsBy.GetAlbumById
{
    public record GetAlbumByIdQuery(Guid Id) : IQuery<GetAlbumByIdResult>;
    public record GetAlbumByIdResult(AlbumDto Album);

    internal class GetAlbumByIdQueryHandler(IRepository<ArchiveContext> repo)
        : IQueryHandler<GetAlbumByIdQuery, GetAlbumByIdResult>
    {
        public async Task<GetAlbumByIdResult> Handle(GetAlbumByIdQuery query, CancellationToken cancellationToken)
        {
            var album = await repo.Filter<Album>(a => a.Id == query.Id)
                .Include(a => a.Bands).ThenInclude(ab => ab.Band)
                .Include(a => a.Countries).ThenInclude(ac => ac.Country)
                .Include(a => a.StreamingLinks)
                .Include(a => a.Tracks).ThenInclude(at => at.Track)
                .Include(a => a.Genres).ThenInclude(ag => ag.Genre)
                .ProjectToType<AlbumDto>()
                .FirstOrDefaultAsync();

            if (album == null) throw new AlbumNotFoundException(query.Id);

            return new GetAlbumByIdResult(album);
        }
    }
}
