using BuildingBlocks.Extentions;
using Library.API.Data;
using Library.Domain.Models;

namespace Library.API.Albums.GetAlbums
{
    public record GetAlbumsQuery(int? PageNumber = 1, int? PageSize = 10)
        : IQuery<PagedResult<AlbumDto>>;

    public class GetAlbumsQueryHandler(IRepository<LibraryContext> repo)
        : IQueryHandler<GetAlbumsQuery, PagedResult<AlbumDto>>
    {
        public async ValueTask<PagedResult<AlbumDto>> Handle(
            GetAlbumsQuery query,
            CancellationToken cancellationToken)
        {
            var albumsQuery = repo.All<Album>()
                .Include(a => a.Bands).ThenInclude(ab => ab.Band)
                .Include(a => a.Countries).ThenInclude(ac => ac.Country)
                .Include(a => a.StreamingLinks)
                .Include(a => a.Tracks).ThenInclude(at => at.Track)
                .Include(a => a.Genres).ThenInclude(ag => ag.Genre)
                .OrderByDescending(a => a.ReleaseDate)
                .ProjectToType<AlbumDto>();

            return await albumsQuery.ToPagedResultAsync(
                query.PageNumber ?? 1,
                query.PageSize ?? 10,
                cancellationToken
            );
        }
    }
}