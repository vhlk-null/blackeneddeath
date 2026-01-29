using Archive.API.Models;

namespace Archive.API.Albums.GetAlbumsBy.GetAlbumByYear
{
    public record GetAlbumsByYearQuery(int ReleaseDate) : IQuery<GetAlbumsByYearResult>;
    public record GetAlbumsByYearResult(IEnumerable<Album> Albums);

    internal class GetAlbumByByYearQueryHandler(IRepository<ArchiveContext> repo)
        : IQueryHandler<GetAlbumsByYearQuery, GetAlbumsByYearResult>
    {
        public async Task<GetAlbumsByYearResult> Handle(GetAlbumsByYearQuery query, CancellationToken cancellationToken)
        {
            var albums = await repo.FilterAsync<Album>(a => a.ReleaseDate == query.ReleaseDate);

            return new GetAlbumsByYearResult(albums);
        }
    }
}
