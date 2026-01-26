namespace Archive.API.Albums.GetAlbumsBy.GetAlbumByYear
{
    public record GetAlbumsByYearQuery(int ReleaseDate) : IQuery<GetAlbumsByYearResult>;
    public record GetAlbumsByYearResult(IEnumerable<Album> Albums);

    internal class GetAlbumByByYearQueryHandler(IRepository<ArchiveContext> repo, ILogger<GetAlbumByByYearQueryHandler> logger)
        : IQueryHandler<GetAlbumsByYearQuery, GetAlbumsByYearResult>
    {
        public async Task<GetAlbumsByYearResult> Handle(GetAlbumsByYearQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetAlbumByByYearQueryHandler.Handle called with {@Query}", query);

            var albums = await repo.FilterAsync<Album>(a => a.ReleaseDate == query.ReleaseDate);

            return new GetAlbumsByYearResult(albums);
        }
    }
}
