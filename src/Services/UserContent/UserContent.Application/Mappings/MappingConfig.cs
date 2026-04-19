namespace UserContent.Application.Mappings;

public static class MappingConfig
{
    public static void RegisterMappings()
    {
        ConfigureGrpcMappings();
        ConfigureFavoriteAlbumMappings();
        ConfigureFavoriteBandMappings();
        ConfigureUserProfileMappings();
    }

    private static void ConfigureGrpcMappings()
    {
        TypeAdapterConfig<GetAlbumResponse, Album>.NewConfig()
            .Map(dest => dest.Id, src => Guid.Parse(src.Id))
            .Map(dest => dest.Title, src => src.Title)
            .Map(dest => dest.CoverUrl, src => src.CoverUrl)
            .Map(dest => dest.ReleaseDate, src => src.ReleaseDate);

        TypeAdapterConfig<GetBandResponse, Band>.NewConfig()
            .Map(dest => dest.BandId, src => Guid.Parse(src.Id))
            .Map(dest => dest.BandName, src => src.Title)
            .Map(dest => dest.LogoUrl, src => src.LogoUrl);
    }

    private static void ConfigureFavoriteAlbumMappings()
    {
        TypeAdapterConfig<FavoriteAlbum, FavoriteAlbumDto>.NewConfig()
            .Map(dest => dest.AlbumTitle, src => src.Album.Title)
            .Map(dest => dest.Slug, src => src.Album.Slug)
            .Map(dest => dest.CoverUrl, src => src.Album.CoverUrl)
            .Map(dest => dest.ReleaseDate, src => src.Album.ReleaseDate)
            .Map(dest => dest.AlbumType, src => ((AlbumType)src.Album.Type).ToString())
            .Map(dest => dest.BandName, src => src.Album.BandNames)
            .Map(dest => dest.BandSlug, src => src.Album.BandSlugs)
            .Map(dest => dest.CountryNames, src => src.Album.CountryNames);
    }

    private static void ConfigureFavoriteBandMappings()
    {
        TypeAdapterConfig<FavoriteBand, FavoriteBandDto>.NewConfig()
            .Map(dest => dest.BandName, src => src.Band.BandName)
            .Map(dest => dest.Slug, src => src.Band.Slug)
            .Map(dest => dest.LogoUrl, src => src.Band.LogoUrl)
            .Map(dest => dest.FormedYear, src => src.Band.FormedYear)
            .Map(dest => dest.PrimaryGenreName, src => src.Band.PrimaryGenreName)
            .Map(dest => dest.CountryNames, src => src.Band.CountryNames);
    }

    private static void ConfigureUserProfileMappings()
    {
        TypeAdapterConfig<UserProfileInfo, UserProfileDto>.NewConfig()
            .Map(dest => dest.FavoriteAlbums, src => src.FavoriteAlbums)
            .Map(dest => dest.FavoriteBands, src => src.FavoriteBands)
            .Map(dest => dest.Collections, src => new List<CollectionDto>());
    }
}
