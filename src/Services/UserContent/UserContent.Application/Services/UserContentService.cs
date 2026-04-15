using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace UserContent.Application.Services;

public class UserContentService(
    IRepository<UserContentContext> repo,
    IHttpContextAccessor httpContextAccessor)
    : IUserContentService
{
    private async Task<UserProfileInfo> EnsureUserProfileAsync(Guid userId, CancellationToken ct)
    {
        UserProfileInfo? profile = await repo.GetByAsync<UserProfileInfo>(u => u.UserId == userId, cancellationToken: ct);
        if (profile is not null)
            return profile;

        ClaimsPrincipal? user = httpContextAccessor.HttpContext?.User;
        string username = user?.FindFirst(ClaimTypes.Name)?.Value ?? userId.ToString();
        string? email = user?.FindFirst(ClaimTypes.Email)?.Value;

        profile = new UserProfileInfo
        {
            UserId = userId,
            Username = username,
            Email = email,
            RegisteredDate = DateTime.UtcNow
        };

        await repo.AddAsync(profile, ct);
        return profile;
    }

    public async Task<UserProfileDto> GetUserProfileAsync(Guid userId, CancellationToken ct = default)
    {
        UserProfileInfo profile = await repo.GetWithIncludesAsync<UserProfileInfo>(
            u => u.UserId == userId,
            q => q.Include(u => u.FavoriteAlbums).ThenInclude(fa => fa.Album)
                  .Include(u => u.FavoriteBands).ThenInclude(fb => fb.Band),
            ct) ?? throw new UserProfileNotFoundException(userId);

        return profile.Adapt<UserProfileDto>();
    }

    public async Task<Guid> AddFavoriteAlbumAsync(Guid userId, Guid albumId, CancellationToken ct = default)
    {
        await EnsureUserProfileAsync(userId, ct);

        Album album = await repo.GetByAsync<Album>(a => a.Id == albumId, cancellationToken: ct)
            ?? throw new NotFoundException("Album", albumId);

        await repo.AddAsync(new FavoriteAlbum { AlbumId = album.Id, UserId = userId, AddedDate = DateTime.UtcNow }, ct);
        await repo.SaveChangesAsync(ct);
        return userId;
    }

    public async Task DeleteFavoriteAlbumAsync(Guid userId, Guid albumId, CancellationToken ct = default)
    {
        FavoriteAlbum fa = await repo.GetByAsync<FavoriteAlbum>(
                               fa => fa.UserId == userId && fa.AlbumId == albumId, cancellationToken: ct)
                           ?? throw new FavoriteAlbumNotFoundException(albumId);

        repo.Delete(fa);
        await repo.SaveChangesAsync(ct);
    }

    public async Task<Guid> AddFavoriteBandAsync(Guid userId, Guid bandId, CancellationToken ct = default)
    {
        await EnsureUserProfileAsync(userId, ct);

        Band band = await repo.GetByAsync<Band>(b => b.BandId == bandId, cancellationToken: ct)
            ?? throw new NotFoundException("Band", bandId);

        await repo.AddAsync(new FavoriteBand { UserId = userId, BandId = band.BandId, AddedDate = DateTime.UtcNow }, ct);
        await repo.SaveChangesAsync(ct);
        return userId;
    }

    public async Task DeleteFavoriteBandAsync(Guid userId, Guid bandId, CancellationToken ct = default)
    {
        FavoriteBand fb = await repo.GetByAsync<FavoriteBand>(
                              fb => fb.UserId == userId && fb.BandId == bandId, cancellationToken: ct)
                          ?? throw new FavoriteBandNotFoundException(bandId);

        repo.Delete(fb);
        await repo.SaveChangesAsync(ct);
    }

    public async Task<(int? UserRating, double? AverageRating, int RatingsCount)> GetAlbumRatingAsync(Guid userId, Guid albumId, CancellationToken ct = default)
    {
        AlbumRating? rating = await repo.GetByAsync<AlbumRating>(
            r => r.UserId == userId && r.AlbumId == albumId, cancellationToken: ct);
        Album? album = await repo.GetByAsync<Album>(a => a.Id == albumId, cancellationToken: ct);
        return (rating?.Rating, album?.AverageRating, album?.RatingsCount ?? 0);
    }

    public async Task<(double? AverageRating, int RatingsCount)> GetAlbumAverageRatingAsync(Guid albumId, CancellationToken ct = default)
    {
        Album? album = await repo.GetByAsync<Album>(a => a.Id == albumId, cancellationToken: ct);
        return (album?.AverageRating, album?.RatingsCount ?? 0);
    }

    public async Task<(int? UserRating, double? AverageRating, int RatingsCount)> GetBandRatingAsync(Guid userId, Guid bandId, CancellationToken ct = default)
    {
        BandRating? rating = await repo.GetByAsync<BandRating>(
            r => r.UserId == userId && r.BandId == bandId, cancellationToken: ct);
        Band? band = await repo.GetByAsync<Band>(b => b.BandId == bandId, cancellationToken: ct);
        return (rating?.Rating, band?.AverageRating, band?.RatingsCount ?? 0);
    }

    public async Task<(double? AverageRating, int RatingsCount)> GetBandAverageRatingAsync(Guid bandId, CancellationToken ct = default)
    {
        Band? band = await repo.GetByAsync<Band>(b => b.BandId == bandId, cancellationToken: ct);
        return (band?.AverageRating, band?.RatingsCount ?? 0);
    }

    public async Task<(double? AverageRating, int RatingsCount)> RateAlbumAsync(Guid userId, Guid albumId, int rating, CancellationToken ct = default)
    {
        await EnsureUserProfileAsync(userId, ct);

        Album album = await repo.GetByAsync<Album>(a => a.Id == albumId, cancellationToken: ct)
            ?? throw new NotFoundException("Album", albumId);

        AlbumRating? existing = await repo.GetByAsync<AlbumRating>(
            r => r.UserId == userId && r.AlbumId == albumId, cancellationToken: ct);

        if (existing is null)
        {
            await repo.AddAsync(new AlbumRating { UserId = userId, AlbumId = albumId, Rating = rating, RatedAt = DateTime.UtcNow }, ct);
            album.AverageRating = ((album.AverageRating ?? 0) * album.RatingsCount + rating) / (album.RatingsCount + 1);
            album.RatingsCount++;
        }
        else
        {
            double oldRating = existing.Rating;
            existing.Rating = rating;
            existing.RatedAt = DateTime.UtcNow;
            album.AverageRating = (album.AverageRating!.Value * album.RatingsCount - oldRating + rating) / album.RatingsCount;
        }

        await repo.SaveChangesAsync(ct);
        return (album.AverageRating, album.RatingsCount);
    }

    public async Task<(double? AverageRating, int RatingsCount)> RateBandAsync(Guid userId, Guid bandId, int rating, CancellationToken ct = default)
    {
        await EnsureUserProfileAsync(userId, ct);

        Band band = await repo.GetByAsync<Band>(b => b.BandId == bandId, cancellationToken: ct)
            ?? throw new NotFoundException("Band", bandId);

        BandRating? existing = await repo.GetByAsync<BandRating>(
            r => r.UserId == userId && r.BandId == bandId, cancellationToken: ct);

        if (existing is null)
        {
            await repo.AddAsync(new BandRating { UserId = userId, BandId = bandId, Rating = rating, RatedAt = DateTime.UtcNow }, ct);
            band.AverageRating = ((band.AverageRating ?? 0) * band.RatingsCount + rating) / (band.RatingsCount + 1);
            band.RatingsCount++;
        }
        else
        {
            double oldRating = existing.Rating;
            existing.Rating = rating;
            existing.RatedAt = DateTime.UtcNow;
            band.AverageRating = (band.AverageRating!.Value * band.RatingsCount - oldRating + rating) / band.RatingsCount;
        }

        await repo.SaveChangesAsync(ct);
        return (band.AverageRating, band.RatingsCount);
    }
}
