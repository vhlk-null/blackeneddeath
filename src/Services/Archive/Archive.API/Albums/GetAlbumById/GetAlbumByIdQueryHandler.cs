namespace Archive.API.Albums.GetAlbums
{
    public record GetAlbumByIdQuery(Guid Id) : IQuery<GetAlbumByIdResult>;
    public record GetAlbumByIdResult(Album Album);

    internal class GetAlbumByIdQueryHandler(IRepository<ArchiveContext> repo)
        : IQueryHandler<GetAlbumByIdQuery, GetAlbumByIdResult>
    {
        public async Task<GetAlbumByIdResult> Handle(GetAlbumByIdQuery query, CancellationToken cancellationToken)
        {
            var album = await repo.GetByAsync<Album>(a=>a.Id == query.Id);

            if(album == null) throw new AlbumNotFoundException(query.Id);

            return new GetAlbumByIdResult(album);
        }
    }
}
