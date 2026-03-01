namespace UserContent.Application.UserContent.FavoriteAlbums.AddFavoriteAlbum;

public class AddFavoriteAlbumCommandHandler(IUserContentRepository repo, ILibraryService libraryService)
    : ICommandHandler<AddAlbumToFavoriteCommand, AddAlbumToFavoriteResult>
{
    public async ValueTask<AddAlbumToFavoriteResult> Handle(AddAlbumToFavoriteCommand request, CancellationToken cancellationToken)
    {
        var album = await repo.GetAlbumAsync(request.AlbumId, cancellationToken);

        if (album is null)
        {
            album = await libraryService.GetAlbumByIdAsync(request.AlbumId, cancellationToken)
                ?? throw new NotFoundException("Album", request.AlbumId);

            await repo.AddAsync(album, cancellationToken);
        }

        var favoriteAlbum = new FavoriteAlbum
        {
            AlbumId = album.Id,
            UserId = request.UserId,
            AddedDate = DateTime.UtcNow
        };

        await repo.AddAsync(favoriteAlbum, cancellationToken);

        return new AddAlbumToFavoriteResult(request.UserId);
    }
}
