namespace Library.Application.Albums.Commands.DeleteAlbums
{
    public record DeleteAlbumCommand(Guid AlbumId) : ICommand<DeleteAlbumResult>;
    public record DeleteAlbumResult(bool IsSuccess);

    public class DeleteAlbumCommandValidator : AbstractValidator<DeleteAlbumCommand>
    {
        public DeleteAlbumCommandValidator()
        {
            RuleFor(x => x.AlbumId).NotEmpty().WithMessage(ValidationMessages.EmptyRequiredField);
        }
    }
}
