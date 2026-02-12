namespace UserContent.API.UserContent.FavoriteAlbums.DeleteUserFavoriteAlbums
{
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

    public class DeleteFavoriteAlbumCommandHandler(IRepository<UserContentContext> repo) : ICommandHandler<DeleteFavoriteAlbumCommand, DeleteFavoriteAlbumResult>
    {
        public async ValueTask<DeleteFavoriteAlbumResult> Handle(DeleteFavoriteAlbumCommand request, CancellationToken cancellationToken)
        {
            var favoriteAlbum = await repo.GetByAsync<FavoriteAlbum>(
                fa => fa.AlbumId == request.AlbumId && fa.UserId == request.UserId, cancellationToken: cancellationToken)
                ?? throw new FavoriteAlbumNotFoundException(request.AlbumId);

            repo.Delete(favoriteAlbum);

            return new DeleteFavoriteAlbumResult(true);
        }
    }
}
