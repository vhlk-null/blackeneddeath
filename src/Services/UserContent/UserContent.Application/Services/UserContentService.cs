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

    public async Task<PaginatedResult<AlbumCardDto>> GetFavoriteAlbumsAsync(Guid userId, int pageIndex, int pageSize, CancellationToken ct = default)
    {
        IQueryable<FavoriteAlbum> favQuery = repo.Filter<FavoriteAlbum>(fa => fa.UserId == userId, asTracked: false);
        IQueryable<Album> albumQuery = repo.Filter<Album>(_ => true, asTracked: false);

        var query = favQuery
            .Join(albumQuery, fa => fa.AlbumId, a => a.Id, (fa, a) => new { fa.AddedDate, Album = a })
            .OrderByDescending(x => x.AddedDate);

        int totalCount = await query.CountAsync(ct);
        var albums = await query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        List<AlbumCardDto> items = albums.Select(x => { var a = x.Album; return new AlbumCardDto(
            a.Id, a.Title, a.Slug, a.CoverUrl, a.ReleaseDate,
            ((AlbumFormat)a.Format).ToString(), ((AlbumType)a.Type).ToString(),
            a.PrimaryGenreName, a.PrimaryGenreSlug,
            a.BandNames, a.BandSlugs, a.CountryNames,
            a.AverageRating, a.RatingsCount); }).ToList();

        return new PaginatedResult<AlbumCardDto>(pageIndex, pageSize, totalCount, items);
    }

    public async Task<bool> IsAlbumFavoriteAsync(Guid userId, Guid albumId, CancellationToken ct = default)
    {
        FavoriteAlbum? fa = await repo.GetByAsync<FavoriteAlbum>(
            fa => fa.UserId == userId && fa.AlbumId == albumId, asTracked: false, cancellationToken: ct);
        return fa is not null;
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

    public async Task<PaginatedResult<BandCardDto>> GetFavoriteBandsAsync(Guid userId, int pageIndex, int pageSize, CancellationToken ct = default)
    {
        IQueryable<FavoriteBand> favQuery = repo.Filter<FavoriteBand>(fb => fb.UserId == userId, asTracked: false);
        IQueryable<Band> bandQuery = repo.Filter<Band>(_ => true, asTracked: false);

        var query = favQuery
            .Join(bandQuery, fb => fb.BandId, b => b.BandId, (fb, b) => new { fb.AddedDate, Band = b })
            .OrderByDescending(x => x.AddedDate);

        int totalCount = await query.CountAsync(ct);
        var bands = await query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        List<BandCardDto> items = bands.Select(x => { var b = x.Band; return new BandCardDto(
            b.BandId, b.BandName, b.Slug, b.LogoUrl, b.FormedYear, b.DisbandedYear,
            ((BandStatus)b.Status).ToString(), b.PrimaryGenreName, b.PrimaryGenreSlug,
            b.CountryNames, b.CountryCodes, b.AverageRating, b.RatingsCount); }).ToList();

        return new PaginatedResult<BandCardDto>(pageIndex, pageSize, totalCount, items);
    }

    public async Task<bool> IsBandFavoriteAsync(Guid userId, Guid bandId, CancellationToken ct = default)
    {
        FavoriteBand? fb = await repo.GetByAsync<FavoriteBand>(
            fb => fb.UserId == userId && fb.BandId == bandId, asTracked: false, cancellationToken: ct);
        return fb is not null;
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
        AlbumReview? review = await repo.GetByAsync<AlbumReview>(
            r => r.UserId == userId && r.AlbumId == albumId, cancellationToken: ct);
        Album? album = await repo.GetByAsync<Album>(a => a.Id == albumId, cancellationToken: ct);
        return (review?.Rating, album?.AverageRating, album?.RatingsCount ?? 0);
    }

    public async Task<(double? AverageRating, int RatingsCount)> GetAlbumAverageRatingAsync(Guid albumId, CancellationToken ct = default)
    {
        Album? album = await repo.GetByAsync<Album>(a => a.Id == albumId, cancellationToken: ct);
        return (album?.AverageRating, album?.RatingsCount ?? 0);
    }

    public async Task<(int? UserRating, double? AverageRating, int RatingsCount)> GetBandRatingAsync(Guid userId, Guid bandId, CancellationToken ct = default)
    {
        BandReview? review = await repo.GetByAsync<BandReview>(
            r => r.UserId == userId && r.BandId == bandId, cancellationToken: ct);
        Band? band = await repo.GetByAsync<Band>(b => b.BandId == bandId, cancellationToken: ct);
        return (review?.Rating, band?.AverageRating, band?.RatingsCount ?? 0);
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

        AlbumReview? review = await repo.GetByAsync<AlbumReview>(
            r => r.UserId == userId && r.AlbumId == albumId, cancellationToken: ct);

        if (review is null)
            throw new NotFoundException("AlbumReview", userId);

        if (review.Rating is null)
        {
            review.Rating = rating;
            review.RatedAt = DateTime.UtcNow;
            album.AverageRating = ((album.AverageRating ?? 0) * album.RatingsCount + rating) / (album.RatingsCount + 1);
            album.RatingsCount++;
        }
        else
        {
            double oldRating = review.Rating.Value;
            review.Rating = rating;
            review.RatedAt = DateTime.UtcNow;
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

        BandReview? review = await repo.GetByAsync<BandReview>(
            r => r.UserId == userId && r.BandId == bandId, cancellationToken: ct);

        if (review is null)
            throw new NotFoundException("BandReview", userId);

        if (review.Rating is null)
        {
            review.Rating = rating;
            review.RatedAt = DateTime.UtcNow;
            band.AverageRating = ((band.AverageRating ?? 0) * band.RatingsCount + rating) / (band.RatingsCount + 1);
            band.RatingsCount++;
        }
        else
        {
            double oldRating = review.Rating.Value;
            review.Rating = rating;
            review.RatedAt = DateTime.UtcNow;
            band.AverageRating = (band.AverageRating!.Value * band.RatingsCount - oldRating + rating) / band.RatingsCount;
        }

        await repo.SaveChangesAsync(ct);
        return (band.AverageRating, band.RatingsCount);
    }

    public async Task<PaginatedResult<AlbumCardDto>> GetTopRatedAlbumsAsync(PaginationRequest pagination, RatingPeriod period, CancellationToken ct = default)
    {
        if (period == RatingPeriod.All)
        {
            IQueryable<Album> query = repo.Filter<Album>(a => a.RatingsCount > 0)
                .OrderByDescending(a => a.AverageRating)
                .ThenByDescending(a => a.RatingsCount);

            long count = await query.LongCountAsync(ct);

            List<AlbumCardDto> data = await query
                .Skip(pagination.PageIndex * pagination.PageSize)
                .Take(pagination.PageSize)
                .Select(a => new AlbumCardDto(
                    a.Id, a.Title, a.Slug, a.CoverUrl, a.ReleaseDate,
                    ((AlbumFormat)a.Format).ToString(), ((AlbumType)a.Type).ToString(),
                    a.PrimaryGenreName, a.PrimaryGenreSlug, a.BandNames, a.BandSlugs,
                    a.CountryNames, a.AverageRating, a.RatingsCount))
                .ToListAsync(ct);

            return new PaginatedResult<AlbumCardDto>(pagination.PageIndex, pagination.PageSize, count, data);
        }

        DateTime cutoff = period == RatingPeriod.Month
            ? DateTime.UtcNow.AddMonths(-1)
            : DateTime.UtcNow.AddYears(-1);

        IQueryable<AlbumReview> ratingsQuery = repo.Filter<AlbumReview>(r => r.Rating != null && r.RatedAt >= cutoff);

        var grouped = ratingsQuery
            .GroupBy(r => r.AlbumId)
            .Select(g => new { AlbumId = g.Key, Avg = g.Average(r => (double)r.Rating!.Value), Count = g.Count() });

        long periodCount = await grouped.LongCountAsync(ct);

        var topIds = await grouped
            .OrderByDescending(g => g.Avg)
            .ThenByDescending(g => g.Count)
            .Skip(pagination.PageIndex * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync(ct);

        List<Guid> ids = topIds.Select(x => x.AlbumId).ToList();

        List<Album> albums = await repo.Filter<Album>(a => ids.Contains(a.Id)).ToListAsync(ct);

        List<AlbumCardDto> periodData = topIds
            .Join(albums, t => t.AlbumId, a => a.Id, (t, a) => new AlbumCardDto(
                a.Id, a.Title, a.Slug, a.CoverUrl, a.ReleaseDate,
                ((AlbumFormat)a.Format).ToString(), ((AlbumType)a.Type).ToString(),
                a.PrimaryGenreName, a.PrimaryGenreSlug, a.BandNames, a.BandSlugs,
                a.CountryNames, t.Avg, t.Count))
            .ToList();

        return new PaginatedResult<AlbumCardDto>(pagination.PageIndex, pagination.PageSize, periodCount, periodData);
    }

    public async Task<PaginatedResult<BandCardDto>> GetTopRatedBandsAsync(PaginationRequest pagination, RatingPeriod period, CancellationToken ct = default)
    {
        if (period == RatingPeriod.All)
        {
            IQueryable<Band> query = repo.Filter<Band>(b => b.RatingsCount > 0)
                .OrderByDescending(b => b.AverageRating)
                .ThenByDescending(b => b.RatingsCount);

            long count = await query.LongCountAsync(ct);

            List<BandCardDto> data = await query
                .Skip(pagination.PageIndex * pagination.PageSize)
                .Take(pagination.PageSize)
                .Select(b => new BandCardDto(
                    b.BandId, b.BandName, b.Slug, b.LogoUrl, b.FormedYear, b.DisbandedYear,
                    ((BandStatus)b.Status).ToString(), b.PrimaryGenreName, b.PrimaryGenreSlug,
                    b.CountryNames, b.CountryCodes, b.AverageRating, b.RatingsCount))
                .ToListAsync(ct);

            return new PaginatedResult<BandCardDto>(pagination.PageIndex, pagination.PageSize, count, data);
        }

        DateTime cutoff = period == RatingPeriod.Month
            ? DateTime.UtcNow.AddMonths(-1)
            : DateTime.UtcNow.AddYears(-1);

        IQueryable<BandReview> ratingsQuery = repo.Filter<BandReview>(r => r.Rating != null && r.RatedAt >= cutoff);

        var grouped = ratingsQuery
            .GroupBy(r => r.BandId)
            .Select(g => new { BandId = g.Key, Avg = g.Average(r => (double)r.Rating!.Value), Count = g.Count() });

        long periodCount = await grouped.LongCountAsync(ct);

        var topIds = await grouped
            .OrderByDescending(g => g.Avg)
            .ThenByDescending(g => g.Count)
            .Skip(pagination.PageIndex * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync(ct);

        List<Guid> ids = topIds.Select(x => x.BandId).ToList();

        List<Band> bands = await repo.Filter<Band>(b => ids.Contains(b.BandId)).ToListAsync(ct);

        List<BandCardDto> periodData = topIds
            .Join(bands, t => t.BandId, b => b.BandId, (t, b) => new BandCardDto(
                b.BandId, b.BandName, b.Slug, b.LogoUrl, b.FormedYear, b.DisbandedYear,
                ((BandStatus)b.Status).ToString(), b.PrimaryGenreName, b.PrimaryGenreSlug,
                b.CountryNames, b.CountryCodes, t.Avg, t.Count))
            .ToList();

        return new PaginatedResult<BandCardDto>(pagination.PageIndex, pagination.PageSize, periodCount, periodData);
    }

    public async Task<PaginatedResult<ReviewDto>> GetAlbumReviewsAsync(Guid albumId, int pageIndex, int pageSize, ReviewOrderBy orderBy, CancellationToken ct = default)
    {
        IQueryable<AlbumReview> baseQuery = repo.Filter<AlbumReview>(r => r.AlbumId == albumId, asTracked: false);

        var ordered = orderBy switch
        {
            ReviewOrderBy.Oldest => baseQuery.OrderBy(r => r.CreatedAt),
            ReviewOrderBy.HighestRated => baseQuery.OrderByDescending(r => r.Rating),
            ReviewOrderBy.LowestRated => baseQuery.OrderBy(r => r.Rating),
            _ => baseQuery.OrderByDescending(r => r.CreatedAt)
        };

        int totalCount = await baseQuery.CountAsync(ct);
        List<ReviewDto> items = await ordered
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .Select(r => new ReviewDto(r.Id, r.UserId, r.Username, r.Title, r.Body, r.CreatedAt, r.Rating))
            .ToListAsync(ct);

        return new PaginatedResult<ReviewDto>(pageIndex, pageSize, totalCount, items);
    }

    public async Task<int> GetAlbumReviewCountAsync(Guid albumId, CancellationToken ct = default)
        => await repo.Filter<AlbumReview>(r => r.AlbumId == albumId, asTracked: false).CountAsync(ct);

    public async Task<ReviewDto> CreateAlbumReviewAsync(CreateAlbumReviewRequest request, CancellationToken ct = default)
    {
        await EnsureUserProfileAsync(request.UserId, ct);

        AlbumReview review = new()
        {
            Id = Guid.NewGuid(),
            AlbumId = request.AlbumId,
            UserId = request.UserId,
            Username = request.Username,
            Title = request.Title,
            Body = request.Body,
            Rating = request.UserRating,
            RatedAt = request.UserRating.HasValue ? DateTime.UtcNow : null,
            CreatedAt = DateTime.UtcNow
        };

        await repo.AddAsync(review, ct);

        if (request.UserRating.HasValue)
        {
            Album album = await repo.GetByAsync<Album>(a => a.Id == request.AlbumId, cancellationToken: ct)
                ?? throw new NotFoundException("Album", request.AlbumId);
            album.AverageRating = ((album.AverageRating ?? 0) * album.RatingsCount + request.UserRating.Value) / (album.RatingsCount + 1);
            album.RatingsCount++;
        }

        await repo.SaveChangesAsync(ct);

        return new ReviewDto(review.Id, review.UserId, review.Username, review.Title, review.Body, review.CreatedAt, review.Rating);
    }

    public async Task<ReviewDto> UpdateAlbumReviewAsync(Guid reviewId, UpdateReviewRequest request, CancellationToken ct = default)
    {
        AlbumReview review = await repo.GetByAsync<AlbumReview>(r => r.Id == reviewId, cancellationToken: ct)
            ?? throw new AlbumReviewNotFoundException(reviewId);

        review.Title = request.Title;
        review.Body = request.Body;

        await repo.SaveChangesAsync(ct);

        return new ReviewDto(review.Id, review.UserId, review.Username, review.Title, review.Body, review.CreatedAt, null);
    }

    public async Task DeleteAlbumReviewAsync(Guid reviewId, CancellationToken ct = default)
    {
        AlbumReview review = await repo.GetByAsync<AlbumReview>(r => r.Id == reviewId, cancellationToken: ct)
            ?? throw new AlbumReviewNotFoundException(reviewId);

        repo.Delete(review);
        await repo.SaveChangesAsync(ct);
    }

    public async Task<PaginatedResult<ReviewDto>> GetBandReviewsAsync(Guid bandId, int pageIndex, int pageSize, ReviewOrderBy orderBy, CancellationToken ct = default)
    {
        IQueryable<BandReview> baseQuery = repo.Filter<BandReview>(r => r.BandId == bandId, asTracked: false);

        var ordered = orderBy switch
        {
            ReviewOrderBy.Oldest => baseQuery.OrderBy(r => r.CreatedAt),
            ReviewOrderBy.HighestRated => baseQuery.OrderByDescending(r => r.Rating),
            ReviewOrderBy.LowestRated => baseQuery.OrderBy(r => r.Rating),
            _ => baseQuery.OrderByDescending(r => r.CreatedAt)
        };

        int totalCount = await baseQuery.CountAsync(ct);
        List<ReviewDto> items = await ordered
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .Select(r => new ReviewDto(r.Id, r.UserId, r.Username, r.Title, r.Body, r.CreatedAt, r.Rating))
            .ToListAsync(ct);

        return new PaginatedResult<ReviewDto>(pageIndex, pageSize, totalCount, items);
    }

    public async Task<int> GetBandReviewCountAsync(Guid bandId, CancellationToken ct = default)
        => await repo.Filter<BandReview>(r => r.BandId == bandId, asTracked: false).CountAsync(ct);

    public async Task<ReviewDto> CreateBandReviewAsync(CreateBandReviewRequest request, CancellationToken ct = default)
    {
        await EnsureUserProfileAsync(request.UserId, ct);

        BandReview review = new()
        {
            Id = Guid.NewGuid(),
            BandId = request.BandId,
            UserId = request.UserId,
            Username = request.Username,
            Title = request.Title,
            Body = request.Body,
            Rating = request.UserRating,
            RatedAt = request.UserRating.HasValue ? DateTime.UtcNow : null,
            CreatedAt = DateTime.UtcNow
        };

        await repo.AddAsync(review, ct);

        if (request.UserRating.HasValue)
        {
            Band band = await repo.GetByAsync<Band>(b => b.BandId == request.BandId, cancellationToken: ct)
                ?? throw new NotFoundException("Band", request.BandId);
            band.AverageRating = ((band.AverageRating ?? 0) * band.RatingsCount + request.UserRating.Value) / (band.RatingsCount + 1);
            band.RatingsCount++;
        }

        await repo.SaveChangesAsync(ct);

        return new ReviewDto(review.Id, review.UserId, review.Username, review.Title, review.Body, review.CreatedAt, review.Rating);
    }

    public async Task<ReviewDto> UpdateBandReviewAsync(Guid reviewId, UpdateReviewRequest request, CancellationToken ct = default)
    {
        BandReview review = await repo.GetByAsync<BandReview>(r => r.Id == reviewId, cancellationToken: ct)
            ?? throw new BandReviewNotFoundException(reviewId);

        review.Title = request.Title;
        review.Body = request.Body;

        await repo.SaveChangesAsync(ct);

        return new ReviewDto(review.Id, review.UserId, review.Username, review.Title, review.Body, review.CreatedAt, null);
    }

    public async Task DeleteBandReviewAsync(Guid reviewId, CancellationToken ct = default)
    {
        BandReview review = await repo.GetByAsync<BandReview>(r => r.Id == reviewId, cancellationToken: ct)
            ?? throw new BandReviewNotFoundException(reviewId);

        repo.Delete(review);
        await repo.SaveChangesAsync(ct);
    }
}
