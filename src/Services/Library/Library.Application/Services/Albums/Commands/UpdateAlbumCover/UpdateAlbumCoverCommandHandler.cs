namespace Library.Application.Services.Albums.Commands.UpdateAlbumCover;

public class UpdateAlbumCoverCommandHandler(ILibraryDbContext context, IStorageService storage)
    : BuildingBlocks.CQRS.ICommandHandler<UpdateAlbumCoverCommand, UpdateAlbumCoverResult>
{
    public async ValueTask<UpdateAlbumCoverResult> Handle(UpdateAlbumCoverCommand command, CancellationToken cancellationToken)
    {
        Album album = await context.Albums.FindAsync([AlbumId.Of(command.AlbumId)], cancellationToken)
                      ?? throw new AlbumNotFoundException(command.AlbumId);

        if (album.CoverUrl is not null)
            await storage.DeleteFileAsync(album.CoverUrl, cancellationToken);

        string folder = $"albums/{Slugify(album.Title)}";
        string extension = Path.GetExtension(command.CoverImageFileName);
        string coverKey = await storage.UploadFileAsync(folder, $"{Guid.NewGuid()}{extension}", command.CoverImage, command.CoverImageContentType, cancellationToken);

        album.Update(album.Title, album.Slug, album.Type, album.AlbumRelease, coverKey, album.LabelId);
        await context.SaveChangesAsync(cancellationToken);

        return new UpdateAlbumCoverResult(true);
    }

    private static string Slugify(string value) =>
        Regex.Replace(value.ToLowerInvariant().Trim(), @"[^a-z0-9]+", "-").Trim('-');
}
