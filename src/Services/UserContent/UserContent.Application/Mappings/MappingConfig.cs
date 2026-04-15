namespace UserContent.Application.Mappings;

public static class MappingConfig
{
    public static void RegisterMappings()
    {
        ConfigureGrpcMappings();
        ConfigureFavoriteAlbumMappings();
        ConfigureFavoriteBandMappings();
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
            .Map(dest => dest.CoverUrl, src => src.Album.CoverUrl)
            .Map(dest => dest.ReleaseDate, src => src.Album.ReleaseDate);
    }

    private static void ConfigureFavoriteBandMappings()
    {
        TypeAdapterConfig<FavoriteBand, FavoriteBandDto>.NewConfig()
            .Map(dest => dest.BandName, src => src.Band.BandName)
            .Map(dest => dest.LogoUrl, src => src.Band.LogoUrl)
            .Map(dest => dest.FormedYear, src => src.Band.FormedYear);
    }
}
