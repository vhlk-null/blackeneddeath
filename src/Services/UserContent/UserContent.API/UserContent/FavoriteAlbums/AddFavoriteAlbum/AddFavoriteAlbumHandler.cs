namespace UserContent.API.UserContent.FavoriteAlbums.AddFavoriteAlbum
{
    public record AddAlbumToFavoriteCommand(Guid albumId, Guid userId) : ICommand<AddAlbumToFavoriteResult>;
    public record AddAlbumToFavoriteResult(Guid userId);

    public class AddAlbumToFavoriteCommandValidator : AbstractValidator<AddAlbumToFavoriteCommand>
    {
        public AddAlbumToFavoriteCommandValidator()
        {
            RuleFor(x => x.albumId).NotEmpty().WithMessage("Album ID is required.");
            RuleFor(x => x.userId).NotEmpty().WithMessage("User ID is required.");
        }
    }

    public class AddFavoriteAlbumCommandHandler(IRepository<UserContentContext> repo) : ICommandHandler<AddAlbumToFavoriteCommand, AddAlbumToFavoriteResult>
    {
        public async ValueTask<AddAlbumToFavoriteResult> Handle(AddAlbumToFavoriteCommand request, CancellationToken cancellationToken)
        {
            // TODO: add album to favorite user's albums
            // TODO: update cache

            var favoriteAlbum = new FavoriteAlbum()
            {
                UserId = request.userId,
                AlbumId = request.albumId,
                AlbumTitle = "test"
            };

            await repo.AddAsync(favoriteAlbum, cancellationToken);

            return new AddAlbumToFavoriteResult(request.userId);
        }
    }
}
