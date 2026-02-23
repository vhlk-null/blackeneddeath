using Library.API.Data;
using Library.API.Exceptions;
using Library.API.Resources.ResourceManagement;
using Library.Domain.Models;

namespace Library.API.Albums.DeleteAlbums
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
        IRepository<LibraryContext> repo,
        ILogger<DeleteAlbumCommandHandler> logger)
        : ICommandHandler<DeleteAlbumCommand, DeleteAlbumResult>
    {
        public async ValueTask<DeleteAlbumResult> Handle(
            DeleteAlbumCommand command,
            CancellationToken cancellationToken)
        {
            var album = await repo.GetByAsync<Album>(
                a => a.Id == command.Id,
                cancellationToken: cancellationToken);

            if (album == null) throw new AlbumNotFoundException(command.Id);

            repo.Delete(album);
            await repo.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Album {AlbumId} deleted successfully", command.Id);

            return new DeleteAlbumResult(true);
        }
    }
}