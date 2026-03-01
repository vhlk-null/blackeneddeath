namespace UserContent.Application.Services;

public class UserContentService(IUserContentRepository repo, ILibraryService libraryService)
    : IUserContentService
{
    public async Task<UserProfileDto> GetUserProfileAsync(Guid userId, CancellationToken ct = default)
    {
        var profile = await repo.GetUserProfileWithDetailsAsync(userId, ct)
            ?? throw new UserProfileNotFoundException(userId);
        return profile.Adapt<UserProfileDto>();
    }

    public async Task<Guid> AddFavoriteAlbumAsync(Guid userId, Guid albumId, CancellationToken ct = default)
    {
        var album = await repo.GetAlbumAsync(albumId, ct);
        if (album is null)
        {
            album = await libraryService.GetAlbumByIdAsync(albumId, ct)
                ?? throw new NotFoundException("Album", albumId);
            await repo.AddAsync(album, ct);
        }

        var favoriteAlbum = new FavoriteAlbum
        {
            AlbumId = album.Id,
            UserId = userId,
            AddedDate = DateTime.UtcNow
        };

        await repo.AddAsync(favoriteAlbum, ct);
        await repo.SaveChangesAsync(ct);
        return userId;
    }

    public async Task DeleteFavoriteAlbumAsync(Guid userId, Guid albumId, CancellationToken ct = default)
    {
        var fa = await repo.GetFavoriteAlbumAsync(userId, albumId, ct)
            ?? throw new FavoriteAlbumNotFoundException(albumId);
        repo.Remove(fa);
        await repo.SaveChangesAsync(ct);
    }

    public async Task<Guid> AddFavoriteBandAsync(Guid userId, Guid bandId, CancellationToken ct = default)
    {
        await repo.AddAsync(new FavoriteBand { UserId = userId, BandId = bandId, AddedDate = DateTime.UtcNow }, ct);
        await repo.SaveChangesAsync(ct);
        return userId;
    }

    public async Task DeleteFavoriteBandAsync(Guid userId, Guid bandId, CancellationToken ct = default)
    {
        var fb = await repo.GetFavoriteBandAsync(userId, bandId, ct)
            ?? throw new FavoriteBandNotFoundException(bandId);
        repo.Remove(fb);
        await repo.SaveChangesAsync(ct);
    }
}
