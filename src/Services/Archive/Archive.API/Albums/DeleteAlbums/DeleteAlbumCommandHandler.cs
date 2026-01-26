namespace Archive.API.Albums.DeleteAlbum
{
    public record DeleteAlbumCommand(Guid Id) : ICommand<DeleteAlbumResult>;
    public record DeleteAlbumResult(bool IsSuccess);

    public class DeleteAlbumCommandValidator : AbstractValidator<DeleteAlbumCommand>
    {
        public DeleteAlbumCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage(ValidationMessages.EmptyRequiredField);
        }
    }

    internal class DeleteAlbumCommandHandler(
        IRepository<ArchiveContext> repo,
        ILogger<DeleteAlbumCommandHandler> logger)
        : ICommandHandler<DeleteAlbumCommand, DeleteAlbumResult>
    {
        public async Task<DeleteAlbumResult> Handle(
            DeleteAlbumCommand command,
            CancellationToken cancellationToken)
        {
            logger.LogInformation("DeleteAlbumCommandHandler.Handle called with {@Command}", command);

            var album = await repo.GetByAsync<Album>(
                a => a.Id == command.Id,
                cancellationToken: cancellationToken);

            if (album == null) throw new AlbumNotFoundException();

            repo.Delete(album);
            await repo.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Album {AlbumId} deleted successfully", command.Id);

            return new DeleteAlbumResult(true);
        }
    }
}