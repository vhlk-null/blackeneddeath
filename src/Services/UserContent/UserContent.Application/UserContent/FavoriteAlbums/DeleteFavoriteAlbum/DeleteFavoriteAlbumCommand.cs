namespace UserContent.Application.UserContent.FavoriteAlbums.DeleteFavoriteAlbum;

public record DeleteFavoriteAlbumCommand(Guid UserId, Guid AlbumId) : ICommand<DeleteFavoriteAlbumResult>;
public record DeleteFavoriteAlbumResult(bool IsSuccess);

public class DeleteFavoriteAlbumCommandValidator : AbstractValidator<DeleteFavoriteAlbumCommand>
{
    public DeleteFavoriteAlbumCommandValidator()
    {
        RuleFor(x => x.AlbumId).NotEmpty().WithMessage("Album ID is required.");
        RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required.");
    }
}
