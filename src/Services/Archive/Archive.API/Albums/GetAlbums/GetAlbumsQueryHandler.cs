using BuildingBlocks.Extentions;

namespace Archive.API.Albums.GetAlbums
{
    public record GetAlbumsQuery(int? PageNumber, int? PageSize = 10) : IQuery<PagedResult<Album>>;

    public class GetAlbumsQueryHandler(IRepository<ArchiveContext> repo) : IQueryHandler<GetAlbumsQuery, PagedResult<Album>>
    {
        public async Task<PagedResult<Album>> Handle(GetAlbumsQuery query, CancellationToken cancellationToken)
        {
            var albumsQuery = repo.All<Album>()
                .OrderByDescending(a => a.ReleaseDate)
                .AsQueryable();

            PagedResult<Album> pagedResult = await albumsQuery.ToPagedResultAsync(
                query.PageNumber ?? 1,
                query.PageSize ?? 10,
                cancellationToken
            );

            return pagedResult;
        }
    }
}
