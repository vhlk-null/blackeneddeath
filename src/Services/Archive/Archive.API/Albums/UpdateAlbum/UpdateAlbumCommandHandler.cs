namespace Archive.API.Albums.UpdateAlbum
{
    public record UpdateAlbumCommand(Guid Id,
    string Title,
    int ReleaseDate,
    AlbumType Type,
    AlbumFormat Format,
    string? Label,
    Guid? CountryId,
    List<Guid> BandIds,
    List<Guid> GenreIds,
    List<Guid> TagIds) : ICommand<UpdateAlbumResult>;

    public record UpdateAlbumResult(bool IsSuccess);

    internal class UpdateAlbumCommandHandler(IRepository<ArchiveContext> repo, ILogger<UpdateAlbumCommandHandler> logger) : ICommandHandler<UpdateAlbumCommand, UpdateAlbumResult>
    {
        public async Task<UpdateAlbumResult> Handle(UpdateAlbumCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("UpdateAlbumCommandHandler.Handle called with {@Command}", command);

            var album = await repo.GetByAsync<Album>(a => a.Id == command.Id) ?? throw new AlbumNotFoundException();

            album.Title = command.Title;
            album.ReleaseDate = command.ReleaseDate;
            album.Type = command.Type;
            album.Format = command.Format;
            album.Label = command.Label;

            repo.Update(album);
            await repo.SaveChangesAsync();

            return new UpdateAlbumResult(true);
        }
    }
}
