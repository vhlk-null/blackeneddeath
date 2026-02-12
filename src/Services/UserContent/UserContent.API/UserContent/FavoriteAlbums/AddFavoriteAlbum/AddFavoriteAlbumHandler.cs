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
            var album = await repo.GetByAsync<Album>(a => a.AlbumId == request.albumId, cancellationToken: cancellationToken);

            if (album is null)
            {
                var albumResponse = protoServiceClient.GetAlbumById(new GetAlbumRequest() { Id = request.albumId.ToString() }, cancellationToken: cancellationToken);
                
                album = albumResponse.Adapt<Album>();
                
                await repo.AddAsync(album, cancellationToken);
            }

            var favoriteAlbum = new FavoriteAlbum()
            {
                AlbumId = album.AlbumId,
                UserId = request.userId
            };

            await repo.AddAsync(favoriteAlbum, cancellationToken);

            return new AddAlbumToFavoriteResult(request.userId);
        }
    }
}
