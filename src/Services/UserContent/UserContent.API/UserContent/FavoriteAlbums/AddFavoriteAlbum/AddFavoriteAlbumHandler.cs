using Archive.Grpc;

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

    public class AddFavoriteAlbumCommandHandler(IRepository<UserContentContext> repo, ArchiveProtoService.ArchiveProtoServiceClient protoServiceClient) : ICommandHandler<AddAlbumToFavoriteCommand, AddAlbumToFavoriteResult>
    {
        public async ValueTask<AddAlbumToFavoriteResult> Handle(AddAlbumToFavoriteCommand request, CancellationToken cancellationToken)
        {
            var albumResponse = protoServiceClient.GetAlbumById(new GetAlbumRequest() { Id = request.albumId.ToString() }, cancellationToken : cancellationToken);
            
            var album = albumResponse.Adapt<Album>();

            var favoriteAlbum = new FavoriteAlbum()
            {
                Album = album,
                UserId = request.userId
            };

            await repo.AddAsync(favoriteAlbum, cancellationToken);

            return new AddAlbumToFavoriteResult(request.userId);
        }
    }
}
