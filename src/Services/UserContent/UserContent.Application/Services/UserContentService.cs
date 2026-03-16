namespace UserContent.Application.Services;

public class UserContentService(IRepository<UserContentContext> repo, ILibraryService libraryService)
    : IUserContentService
{
    public async Task<UserProfileDto> GetUserProfileAsync(Guid userId, CancellationToken ct = default)
    {
        var profile = await repo.GetWithIncludesAsync<UserProfileInfo>(
            u => u.UserId == userId,
            q => q.Include(u => u.FavoriteAlbums).ThenInclude(fa => fa.Album)
                  .Include(u => u.FavoriteBands).ThenInclude(fb => fb.Band),
            ct) ?? throw new UserProfileNotFoundException(userId);

        return profile.Adapt<UserProfileDto>();
    }

    public async Task<Guid> AddFavoriteAlbumAsync(Guid userId, Guid albumId, CancellationToken ct = default)
    {
        var album = await repo.GetByAsync<Album>(a => a.Id == albumId, cancellationToken: ct);
        if (album is null)
        {
            album = await libraryService.GetAlbumByIdAsync(albumId, ct)
                ?? throw new NotFoundException("Album", albumId);
            await repo.AddAsync(album, ct);
        }

        await repo.AddAsync(new FavoriteAlbum { AlbumId = album.Id, UserId = userId, AddedDate = DateTime.UtcNow }, ct);
        await repo.SaveChangesAsync(ct);
        return userId;
    }

    public async Task DeleteFavoriteAlbumAsync(Guid userId, Guid albumId, CancellationToken ct = default)
    {
        var fa = await repo.GetByAsync<FavoriteAlbum>(
            fa => fa.UserId == userId && fa.AlbumId == albumId, cancellationToken: ct)
            ?? throw new FavoriteAlbumNotFoundException(albumId);

        repo.Delete(fa);
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
        var fb = await repo.GetByAsync<FavoriteBand>(
            fb => fb.UserId == userId && fb.BandId == bandId, cancellationToken: ct)
            ?? throw new FavoriteBandNotFoundException(bandId);

        repo.Delete(fb);
        await repo.SaveChangesAsync(ct);
    }
}
