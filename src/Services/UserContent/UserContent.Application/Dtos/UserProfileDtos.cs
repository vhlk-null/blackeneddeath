namespace UserContent.Application.Dtos;

public record AlbumCardDto(
    Guid Id,
    string Title,
    string? Slug,
    string? CoverUrl,
    int ReleaseDate,
    string Format,
    string Type,
    string? PrimaryGenreName,
    string? PrimaryGenreSlug,
    string? BandNames,
    string? BandSlugs,
    string? CountryNames,
    double? AverageRating,
    int RatingsCount);

public record BandCardDto(
    Guid Id,
    string Name,
    string? Slug,
    string? LogoUrl,
    int? FormedYear,
    int? DisbandedYear,
    string Status,
    string? PrimaryGenreName,
    string? PrimaryGenreSlug,
    string? CountryNames,
    string? CountryCodes,
    double? AverageRating,
    int RatingsCount);

public record FavoriteAlbumDto(Guid AlbumId, string AlbumTitle, string? CoverUrl,
    int ReleaseDate, DateTime AddedDate, string? UserReview);

public record FavoriteBandDto(Guid BandId, string BandName, string? LogoUrl,
    int? FormedYear, DateTime AddedDate, bool IsFollowing);

public record UserProfileDto(Guid UserId, string Username, string Email,
    string? AvatarUrl, DateTime RegisteredDate, DateTime? LastLoginDate, string? Bio,
    int FavoriteBandsCount, int FavoriteAlbumsCount, int ReviewsCount,
    List<FavoriteAlbumDto> FavoriteAlbums, List<FavoriteBandDto> FavoriteBands);
