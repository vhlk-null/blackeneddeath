namespace Archive.API.Albums.GetAlbums
{
    public record GetAlbumsQuery() : IQuery<GetAlbumResult>;

    public record GetAlbumResult(IEnumerable<Album> Albums);
    public class GetAlbumsQueryHandler(IRepository<ArchiveContext> repo, ILogger<GetAlbumsQueryHandler> logger)
        : IQueryHandler<GetAlbumsQuery, GetAlbumResult>
    {
        public async Task<GetAlbumResult> Handle(GetAlbumsQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetAlbumsQueryHandler.Handle called with {@Query}", query);

            var albums = await repo.AllAsync<Album>(cancellationToken);

            return new GetAlbumResult(albums);
        }
    }
}
