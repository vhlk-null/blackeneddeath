using Library.Grpc;

namespace UserContent.API.UserContent.FavoriteAlbums.AddFavoriteAlbum
{
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

    public class AddFavoriteAlbumCommandHandler(IRepository<UserContentContext> repo, LibraryProtoService.LibraryProtoServiceClient protoServiceClient) : ICommandHandler<AddAlbumToFavoriteCommand, AddAlbumToFavoriteResult>
    {
        public async ValueTask<AddAlbumToFavoriteResult> Handle(AddAlbumToFavoriteCommand request, CancellationToken cancellationToken)
        {
            var album = await repo.GetByAsync<Album>(a => a.Id == request.AlbumId, cancellationToken: cancellationToken);

            if (album is null)
            {
                var albumResponse = protoServiceClient.GetAlbumById(new GetAlbumRequest() { Id = request.AlbumId.ToString() }, cancellationToken: cancellationToken);
                
                album = albumResponse.Adapt<Album>();
                
                await repo.AddAsync(album, cancellationToken);
            }

            var favoriteAlbum = new FavoriteAlbum()
            {
                AlbumId = album.Id,
                UserId = request.UserId,
                AddedDate = DateTime.UtcNow
            };

            await repo.AddAsync(favoriteAlbum, cancellationToken);

            return new AddAlbumToFavoriteResult(request.UserId);
        }
    }
}
