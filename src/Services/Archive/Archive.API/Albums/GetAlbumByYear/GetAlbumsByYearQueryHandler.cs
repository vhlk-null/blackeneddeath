namespace Archive.API.Albums.GetAlbumByYear
{
    public record GetAlbumsByYearQuery(int ReleaseDate) : IQuery<GetAlbumsByYearResult>;
    public record GetAlbumsByYearResult(IEnumerable<Album> Albums);

    internal class GetAlbumByIdQueryHandler(IRepository<ArchiveContext> repo, ILogger<GetAlbumByIdQueryHandler> logger)
        : IQueryHandler<GetAlbumsByYearQuery, GetAlbumsByYearResult>
    {
        public async Task<GetAlbumsByYearResult> Handle(GetAlbumsByYearQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetAlbumByIdQueryHandler.Handle called with {@Query}", query);

            var albums = await repo.FilterAsync<Album>(a => a.ReleaseDate == query.ReleaseDate);

            if (albums == null) throw new AlbumNotFoundException();

            return new GetAlbumsByYearResult(albums);
        }
    }
}
