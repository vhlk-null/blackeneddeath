using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace UserContent.Application.Services;

public class UserContentService(
    IRepository<UserContentContext> repo,
    IStorageService storage,
    IStorageUrlResolver urlResolver,
    IHttpContextAccessor httpContextAccessor,
    ILogger<UserContentService> logger)
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

        List<Collection> collections = await repo.Filter<Collection>(c => c.UserId == userId, asTracked: false)
            .Include(c => c.CollectionAlbums).ThenInclude(ca => ca.Album)
            .Include(c => c.CollectionBands).ThenInclude(cb => cb.Band)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync(ct);

        List<CollectionDto> collectionDtos = collections.Select(c => new CollectionDto(
            c.Id, c.UserId, c.Name, c.Description, ((CollectionType)c.Type).ToString(), c.CreatedAt,
            c.Type == (int)CollectionType.Albums ? c.CollectionAlbums.Count : c.CollectionBands.Count,
            urlResolver.Resolve(c.CoverUrl))).ToList();

        UserProfileDto dto = profile.Adapt<UserProfileDto>();
        return dto with { Collections = collectionDtos };
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
            a.AverageRating, a.RatingsCount, a.IsExplicit); }).ToList();

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

    public async Task<(int? UserRating, double? AverageRating, int RatingsCount, bool IsExplicit)> GetAlbumRatingAsync(Guid userId, Guid albumId, CancellationToken ct = default)
    {
        AlbumReview? review = await repo.GetByAsync<AlbumReview>(
            r => r.UserId == userId && r.AlbumId == albumId, cancellationToken: ct);
        Album? album = await repo.GetByAsync<Album>(a => a.Id == albumId, cancellationToken: ct);
        return (review?.Rating, album?.AverageRating, album?.RatingsCount ?? 0, album?.IsExplicit ?? false);
    }

    public async Task<(double? AverageRating, int RatingsCount, bool IsExplicit)> GetAlbumAverageRatingAsync(Guid albumId, CancellationToken ct = default)
    {
        Album? album = await repo.GetByAsync<Album>(a => a.Id == albumId, cancellationToken: ct);
        return (album?.AverageRating, album?.RatingsCount ?? 0, album?.IsExplicit ?? false);
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

    public async Task<(double? AverageRating, int RatingsCount, bool IsExplicit)> RateAlbumAsync(Guid userId, Guid albumId, int rating, CancellationToken ct = default)
    {
        ClaimsPrincipal? user = httpContextAccessor.HttpContext?.User;
        string username = user?.FindFirst(ClaimTypes.Name)?.Value ?? userId.ToString();

        ReviewDto review = await CreateAlbumReviewAsync(
            new CreateAlbumReviewRequest(albumId, userId, username, null, null, rating), ct);

        Album album = await repo.GetByAsync<Album>(a => a.Id == albumId, asTracked: false, cancellationToken: ct)
            ?? throw new NotFoundException("Album", albumId);

        return (album.AverageRating, album.RatingsCount, album.IsExplicit);
    }

    public async Task<(double? AverageRating, int RatingsCount)> RateBandAsync(Guid userId, Guid bandId, int rating, CancellationToken ct = default)
    {
        ClaimsPrincipal? user = httpContextAccessor.HttpContext?.User;
        string username = user?.FindFirst(ClaimTypes.Name)?.Value ?? userId.ToString();

        ReviewDto review = await CreateBandReviewAsync(
            new CreateBandReviewRequest(bandId, userId, username, null, null, rating), ct);

        Band band = await repo.GetByAsync<Band>(b => b.BandId == bandId, asTracked: false, cancellationToken: ct)
            ?? throw new NotFoundException("Band", bandId);

        return (band.AverageRating, band.RatingsCount);
    }

    public async Task<PaginatedResult<AlbumCardDto>> GetTopRatedAlbumsAsync(PaginationRequest pagination, RatingPeriod period, CancellationToken ct = default)
    {
        if (period == RatingPeriod.All)
        {
            IQueryable<Album> query = repo.All<Album>()
                .Where(a => a.Slug != null && a.Slug != "")
                .OrderByDescending(a => a.AverageRating.HasValue)
                .ThenByDescending(a => a.AverageRating)
                .ThenByDescending(a => a.RatingsCount);

            long count = await query.LongCountAsync(ct);

            List<AlbumCardDto> data = await query
                .Skip(pagination.PageIndex * pagination.PageSize)
                .Take(pagination.PageSize)
                .Select(a => new AlbumCardDto(
                    a.Id, a.Title, a.Slug, a.CoverUrl, a.ReleaseDate,
                    ((AlbumFormat)a.Format).ToString(), ((AlbumType)a.Type).ToString(),
                    a.PrimaryGenreName, a.PrimaryGenreSlug, a.BandNames, a.BandSlugs,
                    a.CountryNames, a.AverageRating, a.RatingsCount, a.IsExplicit))
                .ToListAsync(ct);

            return new PaginatedResult<AlbumCardDto>(pagination.PageIndex, pagination.PageSize, count, data);
        }

        DateTime cutoff = period == RatingPeriod.Month
            ? DateTime.UtcNow.AddMonths(-1)
            : DateTime.UtcNow.AddYears(-1);

        var grouped = repo.Filter<AlbumReview>(r => r.Rating != null && r.RatedAt >= cutoff)
            .GroupBy(r => r.AlbumId)
            .Select(g => new { AlbumId = g.Key, Avg = (double?)g.Average(r => (double)r.Rating!.Value), Count = g.Count() });

        var periodData = await repo.All<Album>()
            .Where(a => a.Slug != null && a.Slug != "")
            .GroupJoin(grouped, a => a.Id, g => g.AlbumId, (a, ratings) => new { Album = a, Ratings = ratings })
            .SelectMany(x => x.Ratings.DefaultIfEmpty(), (x, r) => new
            {
                x.Album,
                Avg = r != null ? r.Avg : null,
                Count = r != null ? r.Count : 0
            })
            .OrderByDescending(x => x.Avg.HasValue)
            .ThenByDescending(x => x.Avg)
            .ThenByDescending(x => x.Count)
            .Select(x => new AlbumCardDto(
                x.Album.Id, x.Album.Title, x.Album.Slug, x.Album.CoverUrl, x.Album.ReleaseDate,
                ((AlbumFormat)x.Album.Format).ToString(), ((AlbumType)x.Album.Type).ToString(),
                x.Album.PrimaryGenreName, x.Album.PrimaryGenreSlug, x.Album.BandNames, x.Album.BandSlugs,
                x.Album.CountryNames, x.Avg, x.Count, x.Album.IsExplicit))
            .ToListAsync(ct);

        long periodCount = periodData.Count;

        List<AlbumCardDto> page = periodData
            .Skip(pagination.PageIndex * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToList();

        return new PaginatedResult<AlbumCardDto>(pagination.PageIndex, pagination.PageSize, periodCount, page);
    }

    public async Task<PaginatedResult<BandCardDto>> GetTopRatedBandsAsync(PaginationRequest pagination, RatingPeriod period, CancellationToken ct = default)
    {
        if (period == RatingPeriod.All)
        {
            IQueryable<Band> query = repo.All<Band>()
                .Where(b => b.Slug != null && b.Slug != "")
                .OrderByDescending(b => b.AverageRating.HasValue)
                .ThenByDescending(b => b.AverageRating)
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

        var grouped = repo.Filter<BandReview>(r => r.Rating != null && r.RatedAt >= cutoff)
            .GroupBy(r => r.BandId)
            .Select(g => new { BandId = g.Key, Avg = (double?)g.Average(r => (double)r.Rating!.Value), Count = g.Count() });

        var periodData = await repo.All<Band>()
            .Where(b => b.Slug != null && b.Slug != "")
            .GroupJoin(grouped, b => b.BandId, g => g.BandId, (b, ratings) => new { Band = b, Ratings = ratings })
            .SelectMany(x => x.Ratings.DefaultIfEmpty(), (x, r) => new
            {
                x.Band,
                Avg = r != null ? r.Avg : null,
                Count = r != null ? r.Count : 0
            })
            .OrderByDescending(x => x.Avg.HasValue)
            .ThenByDescending(x => x.Avg)
            .ThenByDescending(x => x.Count)
            .Select(x => new BandCardDto(
                x.Band.BandId, x.Band.BandName, x.Band.Slug, x.Band.LogoUrl, x.Band.FormedYear, x.Band.DisbandedYear,
                ((BandStatus)x.Band.Status).ToString(), x.Band.PrimaryGenreName, x.Band.PrimaryGenreSlug,
                x.Band.CountryNames, x.Band.CountryCodes, x.Avg, x.Count))
            .ToListAsync(ct);

        long periodCount = periodData.Count;

        List<BandCardDto> page = periodData
            .Skip(pagination.PageIndex * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToList();

        return new PaginatedResult<BandCardDto>(pagination.PageIndex, pagination.PageSize, periodCount, page);
    }

    public async Task<PaginatedResult<ReviewDto>> GetBandAlbumReviewsAsync(Guid bandId, int pageIndex, int pageSize, ReviewOrderBy orderBy, CancellationToken ct = default)
    {
        string bandIdStr = bandId.ToString();

        IQueryable<AlbumReview> baseQuery = repo.Filter<AlbumReview>(
            r => r.Album != null && r.Album.BandIds != null && r.Album.BandIds.Contains(bandIdStr)
                 && (!string.IsNullOrEmpty(r.Title) || !string.IsNullOrEmpty(r.Body)),
            asTracked: false);

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

    public async Task<PaginatedResult<ReviewDto>> GetAlbumReviewsAsync(Guid albumId, int pageIndex, int pageSize, ReviewOrderBy orderBy, CancellationToken ct = default)
    {
        IQueryable<AlbumReview> baseQuery = repo.Filter<AlbumReview>(r => r.AlbumId == albumId && ((!string.IsNullOrEmpty(r.Title)) || (!string.IsNullOrEmpty(r.Body))), asTracked: false);

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
        => await repo.Filter<AlbumReview>(r => r.AlbumId == albumId && ((!string.IsNullOrEmpty(r.Title)) || (!string.IsNullOrEmpty(r.Body))), asTracked: false).CountAsync(ct);

    public async Task<ReviewDto> CreateAlbumReviewAsync(CreateAlbumReviewRequest request, CancellationToken ct = default)
    {
        request = request with { UserRating = request.UserRating is > 0 ? request.UserRating : null };
        await EnsureUserProfileAsync(request.UserId, ct);

        AlbumReview? existing = await repo.GetByAsync<AlbumReview>(
            r => r.UserId == request.UserId && r.AlbumId == request.AlbumId, cancellationToken: ct);

        if (existing is not null)
        {
            existing.Title = request.Title;
            existing.Body = request.Body;

            if (request.UserRating.HasValue)
            {
                Album album = await repo.GetByAsync<Album>(a => a.Id == request.AlbumId, cancellationToken: ct)
                    ?? throw new NotFoundException("Album", request.AlbumId);

                if (existing.Rating is null or 0)
                {
                    double newAvg = ((album.AverageRating ?? 0) * album.RatingsCount + request.UserRating.Value) / (album.RatingsCount + 1);
                    album.AverageRating = newAvg;
                    album.RatingsCount++;
                    existing.Rating = null;
                }
                else
                {
                    double newAvg = album.RatingsCount > 0 && double.IsFinite(album.AverageRating ?? 0)
                        ? (album.AverageRating!.Value * album.RatingsCount - existing.Rating.Value + request.UserRating.Value) / album.RatingsCount
                        : request.UserRating.Value;
                    album.AverageRating = newAvg;
                }

                existing.Rating = request.UserRating;
                existing.RatedAt = DateTime.UtcNow;
                repo.Update(album);
            }

            await repo.SaveChangesAsync(ct);
            return new ReviewDto(existing.Id, existing.UserId, existing.Username, existing.Title, existing.Body, existing.CreatedAt, existing.Rating);
        }

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
            double newAvg = ((album.AverageRating ?? 0) * album.RatingsCount + request.UserRating.Value) / (album.RatingsCount + 1);
            album.AverageRating = newAvg;
            album.RatingsCount++;
            repo.Update(album);
        }

        await repo.SaveChangesAsync(ct);
        return new ReviewDto(review.Id, review.UserId, review.Username, review.Title, review.Body, review.CreatedAt, review.Rating);
    }

    public async Task<ReviewDto> UpdateAlbumReviewAsync(Guid reviewId, UpdateReviewRequest request, CancellationToken ct = default)
    {
        request = request with { UserRating = request.UserRating is > 0 ? request.UserRating : null };
        AlbumReview review = await repo.GetByAsync<AlbumReview>(r => r.Id == reviewId, cancellationToken: ct)
            ?? throw new AlbumReviewNotFoundException(reviewId);

        review.Title = request.Title;
        review.Body = request.Body;

        if (request.UserRating.HasValue)
        {
            Album album = await repo.GetByAsync<Album>(a => a.Id == review.AlbumId, cancellationToken: ct)
                ?? throw new NotFoundException("Album", review.AlbumId);

            if (review.Rating is null)
            {
                album.AverageRating = ((album.AverageRating ?? 0) * album.RatingsCount + request.UserRating.Value) / (album.RatingsCount + 1);
                album.RatingsCount++;
            }
            else
            {
                album.AverageRating = (album.AverageRating!.Value * album.RatingsCount - review.Rating.Value + request.UserRating.Value) / album.RatingsCount;
            }

            review.Rating = request.UserRating;
            review.RatedAt = DateTime.UtcNow;
            repo.Update(album);
        }

        await repo.SaveChangesAsync(ct);

        return new ReviewDto(review.Id, review.UserId, review.Username, review.Title, review.Body, review.CreatedAt, review.Rating);
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
        IQueryable<BandReview> baseQuery = repo.Filter<BandReview>(r => r.BandId == bandId && ((!string.IsNullOrEmpty(r.Title)) || (!string.IsNullOrEmpty(r.Body))), asTracked: false);

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
        => await repo.Filter<BandReview>(r => r.BandId == bandId && ((!string.IsNullOrEmpty(r.Title)) || (!string.IsNullOrEmpty(r.Body))), asTracked: false).CountAsync(ct);

    public async Task<ReviewDto> CreateBandReviewAsync(CreateBandReviewRequest request, CancellationToken ct = default)
    {
        request = request with { UserRating = request.UserRating is > 0 ? request.UserRating : null };
        await EnsureUserProfileAsync(request.UserId, ct);

        BandReview? existing = await repo.GetByAsync<BandReview>(
            r => r.UserId == request.UserId && r.BandId == request.BandId, cancellationToken: ct);

        if (existing is not null)
        {
            existing.Title = request.Title;
            existing.Body = request.Body;

            if (request.UserRating.HasValue)
            {
                Band band = await repo.GetByAsync<Band>(b => b.BandId == request.BandId, cancellationToken: ct)
                    ?? throw new NotFoundException("Band", request.BandId);

                if (existing.Rating is null or 0)
                {
                    double newAvg = ((band.AverageRating ?? 0) * band.RatingsCount + request.UserRating.Value) / (band.RatingsCount + 1);
                    band.AverageRating = newAvg;
                    band.RatingsCount++;
                    existing.Rating = null;
                }
                else
                {
                    double newAvg = band.RatingsCount > 0 && double.IsFinite(band.AverageRating ?? 0)
                        ? (band.AverageRating!.Value * band.RatingsCount - existing.Rating.Value + request.UserRating.Value) / band.RatingsCount
                        : request.UserRating.Value;
                    band.AverageRating = newAvg;
                }

                existing.Rating = request.UserRating;
                existing.RatedAt = DateTime.UtcNow;
                repo.Update(band);
            }

            await repo.SaveChangesAsync(ct);
            return new ReviewDto(existing.Id, existing.UserId, existing.Username, existing.Title, existing.Body, existing.CreatedAt, existing.Rating);
        }

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
            double newBandAvg = ((band.AverageRating ?? 0) * band.RatingsCount + request.UserRating.Value) / (band.RatingsCount + 1);
            band.AverageRating = newBandAvg;
            band.RatingsCount++;
            repo.Update(band);
        }

        await repo.SaveChangesAsync(ct);
        return new ReviewDto(review.Id, review.UserId, review.Username, review.Title, review.Body, review.CreatedAt, review.Rating);
    }

    public async Task<ReviewDto> UpdateBandReviewAsync(Guid reviewId, UpdateReviewRequest request, CancellationToken ct = default)
    {
        request = request with { UserRating = request.UserRating is > 0 ? request.UserRating : null };
        BandReview review = await repo.GetByAsync<BandReview>(r => r.Id == reviewId, cancellationToken: ct)
            ?? throw new BandReviewNotFoundException(reviewId);

        review.Title = request.Title;
        review.Body = request.Body;

        if (request.UserRating.HasValue)
        {
            Band band = await repo.GetByAsync<Band>(b => b.BandId == review.BandId, cancellationToken: ct)
                ?? throw new NotFoundException("Band", review.BandId);

            if (review.Rating is null)
            {
                band.AverageRating = ((band.AverageRating ?? 0) * band.RatingsCount + request.UserRating.Value) / (band.RatingsCount + 1);
                band.RatingsCount++;
            }
            else
            {
                band.AverageRating = (band.AverageRating!.Value * band.RatingsCount - review.Rating.Value + request.UserRating.Value) / band.RatingsCount;
            }

            review.Rating = request.UserRating;
            review.RatedAt = DateTime.UtcNow;
            repo.Update(band);
        }

        await repo.SaveChangesAsync(ct);

        return new ReviewDto(review.Id, review.UserId, review.Username, review.Title, review.Body, review.CreatedAt, review.Rating);
    }

    public async Task DeleteBandReviewAsync(Guid reviewId, CancellationToken ct = default)
    {
        BandReview review = await repo.GetByAsync<BandReview>(r => r.Id == reviewId, cancellationToken: ct)
            ?? throw new BandReviewNotFoundException(reviewId);

        repo.Delete(review);
        await repo.SaveChangesAsync(ct);
    }

    public async Task<List<CollectionSummaryDto>> GetUserCollectionsAsync(Guid userId, CancellationToken ct = default)
    {
        List<Collection> collections = await repo.Filter<Collection>(c => c.UserId == userId, asTracked: false)
            .Include(c => c.CollectionAlbums).ThenInclude(ca => ca.Album)
            .Include(c => c.CollectionBands).ThenInclude(cb => cb.Band)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync(ct);

        return collections.Select(c => new CollectionSummaryDto(
            c.Id, c.UserId, c.Name, ((CollectionType)c.Type).ToString(),
            c.Type == (int)CollectionType.Albums ? c.CollectionAlbums.Count : c.CollectionBands.Count,
            c.CollectionAlbums.Select(ca => new CollectionAlbumItemDto(ca.Album.Id, ca.Album.Title, ca.Album.Slug, ca.Album.CoverUrl, ca.Album.ReleaseDate, ca.Album.BandNames)).ToList(),
            c.CollectionBands.Select(cb => new CollectionBandItemDto(cb.Band.BandId, cb.Band.BandName, cb.Band.Slug, cb.Band.LogoUrl, cb.Band.FormedYear)).ToList(),
            urlResolver.Resolve(c.CoverUrl))).ToList();
    }

    public async Task<CollectionDetailDto> GetCollectionAsync(Guid collectionId, CancellationToken ct = default)
    {
        Collection collection = await repo.Filter<Collection>(c => c.Id == collectionId, asTracked: false)
            .Include(c => c.CollectionAlbums).ThenInclude(ca => ca.Album)
            .Include(c => c.CollectionBands).ThenInclude(cb => cb.Band)
            .FirstOrDefaultAsync(ct) ?? throw new CollectionNotFoundException(collectionId);

        List<CollectionAlbumItemDto> albums = collection.CollectionAlbums
            .Select(ca => new CollectionAlbumItemDto(ca.Album.Id, ca.Album.Title, ca.Album.Slug, ca.Album.CoverUrl, ca.Album.ReleaseDate, ca.Album.BandNames))
            .ToList();

        List<CollectionBandItemDto> bands = collection.CollectionBands
            .Select(cb => new CollectionBandItemDto(cb.Band.BandId, cb.Band.BandName, cb.Band.Slug, cb.Band.LogoUrl, cb.Band.FormedYear))
            .ToList();

        int count = collection.Type == (int)CollectionType.Albums ? albums.Count : bands.Count;
        return new CollectionDetailDto(collection.Id, collection.UserId, collection.Name, collection.Description, ((CollectionType)collection.Type).ToString(), collection.CreatedAt, count, albums, bands, urlResolver.Resolve(collection.CoverUrl));
    }

    public async Task<CollectionDto> CreateCollectionAsync(CreateCollectionRequest request, Stream? coverImage, string? coverContentType, string? coverFileName, CancellationToken ct = default)
    {
        await EnsureUserProfileAsync(request.UserId, ct);

        string? coverUrl = null;
        if (coverImage is not null && coverFileName is not null && coverContentType is not null)
        {
            string slug = request.Name.ToLowerInvariant().Replace(" ", "-");
            coverUrl = await storage.UploadFileAsync($"collections/{request.UserId}", $"{slug}{Path.GetExtension(coverFileName)}", coverImage, coverContentType, ct);
        }

        Collection collection = new()
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            Name = request.Name,
            Description = request.Description,
            Type = (int)request.Type,
            CoverUrl = coverUrl,
            CreatedAt = DateTime.UtcNow
        };

        await repo.AddAsync(collection, ct);
        await repo.SaveChangesAsync(ct);

        return new CollectionDto(collection.Id, collection.UserId, collection.Name, collection.Description, request.Type.ToString(), collection.CreatedAt, 0, urlResolver.Resolve(collection.CoverUrl));
    }

    public async Task<CollectionDto> UpdateCollectionCoverAsync(Guid collectionId, Stream coverImage, string coverContentType, string coverFileName, CancellationToken ct = default)
    {
        Collection collection = await repo.Filter<Collection>(c => c.Id == collectionId)
            .Include(c => c.CollectionAlbums)
            .Include(c => c.CollectionBands)
            .FirstOrDefaultAsync(ct) ?? throw new CollectionNotFoundException(collectionId);

        if (collection.CoverUrl is not null)
            await storage.DeleteFileAsync(collection.CoverUrl, ct);

        string slug = collection.Name.ToLowerInvariant().Replace(" ", "-");
        collection.CoverUrl = await storage.UploadFileAsync($"collections/{collection.UserId}", $"{slug}{Path.GetExtension(coverFileName)}", coverImage, coverContentType, ct);

        repo.Update(collection);
        await repo.SaveChangesAsync(ct);

        int count = collection.Type == (int)CollectionType.Albums ? collection.CollectionAlbums.Count : collection.CollectionBands.Count;
        return new CollectionDto(collection.Id, collection.UserId, collection.Name, collection.Description, ((CollectionType)collection.Type).ToString(), collection.CreatedAt,
            count, urlResolver.Resolve(collection.CoverUrl));
    }

    public async Task<CollectionDto> UpdateCollectionAsync(Guid collectionId, UpdateCollectionRequest request, CancellationToken ct = default)
    {
        Collection collection = await repo.Filter<Collection>(c => c.Id == collectionId)
            .Include(c => c.CollectionAlbums).ThenInclude(ca => ca.Album)
            .Include(c => c.CollectionBands).ThenInclude(cb => cb.Band)
            .FirstOrDefaultAsync(ct) ?? throw new CollectionNotFoundException(collectionId);

        collection.Name = request.Name;
        collection.Description = request.Description;

        repo.Update(collection);
        await repo.SaveChangesAsync(ct);

        int count = collection.Type == (int)CollectionType.Albums ? collection.CollectionAlbums.Count : collection.CollectionBands.Count;
        return new CollectionDto(collection.Id, collection.UserId, collection.Name, collection.Description, ((CollectionType)collection.Type).ToString(), collection.CreatedAt,
            count, urlResolver.Resolve(collection.CoverUrl));
    }

    public async Task DeleteCollectionAsync(Guid collectionId, CancellationToken ct = default)
    {
        Collection collection = await repo.GetByAsync<Collection>(c => c.Id == collectionId, cancellationToken: ct)
            ?? throw new CollectionNotFoundException(collectionId);

        repo.Delete(collection);
        await repo.SaveChangesAsync(ct);
    }

    public async Task AddAlbumToCollectionAsync(Guid collectionId, Guid albumId, CancellationToken ct = default)
    {
        Collection collection = await repo.GetByAsync<Collection>(c => c.Id == collectionId, cancellationToken: ct)
            ?? throw new CollectionNotFoundException(collectionId);

        if (collection.Type != (int)CollectionType.Albums)
            throw new BadRequestException("This collection is for bands only.");

        Album album = await repo.GetByAsync<Album>(a => a.Id == albumId, cancellationToken: ct)
            ?? throw new NotFoundException("Album", albumId);

        CollectionAlbum? existing = await repo.GetByAsync<CollectionAlbum>(
            ca => ca.CollectionId == collectionId && ca.AlbumId == albumId, cancellationToken: ct);

        if (existing is not null) return;

        await repo.AddAsync(new CollectionAlbum { CollectionId = collectionId, AlbumId = albumId, AddedDate = DateTime.UtcNow }, ct);
        await repo.SaveChangesAsync(ct);
    }

    public async Task RemoveAlbumFromCollectionAsync(Guid collectionId, Guid albumId, CancellationToken ct = default)
    {
        CollectionAlbum ca = await repo.GetByAsync<CollectionAlbum>(
            ca => ca.CollectionId == collectionId && ca.AlbumId == albumId, cancellationToken: ct)
            ?? throw new NotFoundException("CollectionAlbum", albumId);

        repo.Delete(ca);
        await repo.SaveChangesAsync(ct);
    }

    public async Task AddBandToCollectionAsync(Guid collectionId, Guid bandId, CancellationToken ct = default)
    {
        Collection collection = await repo.GetByAsync<Collection>(c => c.Id == collectionId, cancellationToken: ct)
            ?? throw new CollectionNotFoundException(collectionId);

        if (collection.Type != (int)CollectionType.Bands)
            throw new BadRequestException("This collection is for albums only.");

        Band band = await repo.GetByAsync<Band>(b => b.BandId == bandId, cancellationToken: ct)
            ?? throw new NotFoundException("Band", bandId);

        CollectionBand? existing = await repo.GetByAsync<CollectionBand>(
            cb => cb.CollectionId == collectionId && cb.BandId == bandId, cancellationToken: ct);

        if (existing is not null) return;

        await repo.AddAsync(new CollectionBand { CollectionId = collectionId, BandId = bandId, AddedDate = DateTime.UtcNow }, ct);
        await repo.SaveChangesAsync(ct);
    }

    public async Task RemoveBandFromCollectionAsync(Guid collectionId, Guid bandId, CancellationToken ct = default)
    {
        CollectionBand cb = await repo.GetByAsync<CollectionBand>(
            cb => cb.CollectionId == collectionId && cb.BandId == bandId, cancellationToken: ct)
            ?? throw new NotFoundException("CollectionBand", bandId);

        repo.Delete(cb);
        await repo.SaveChangesAsync(ct);
    }
}
