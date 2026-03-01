namespace UserContent.Application.UserContent.FavoriteAlbums.AddFavoriteAlbum;

public record AddAlbumToFavoriteCommand(Guid AlbumId, Guid UserId) : ICommand<AddAlbumToFavoriteResult>;
public record AddAlbumToFavoriteResult(Guid UserId);

public class AddAlbumToFavoriteCommandValidator : AbstractValidator<AddAlbumToFavoriteCommand>
{
    public AddAlbumToFavoriteCommandValidator()
    {
        RuleFor(x => x.AlbumId).NotEmpty().WithMessage("Album ID is required.");
        RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required.");
    }
}
