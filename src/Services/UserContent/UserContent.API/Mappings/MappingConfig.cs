using UserContent.API.UserContent.UserProfile.GetUserProfile;

namespace UserContent.API.Mappings
{
    public static class MappingConfig
    {
        public static void RegisterMappings()
        {
            ConfigureFavoriteAlbumMappings();
            ConfigureFavoriteBandMappings();
        }

        private static void ConfigureFavoriteAlbumMappings()
        {
            TypeAdapterConfig<FavoriteAlbum, FavoriteAlbumDto>.NewConfig()
                .Map(dest => dest.AlbumTitle, src => src.Album.AlbumTitle)
                .Map(dest => dest.CoverUrl, src => src.Album.CoverUrl)
                .Map(dest => dest.ReleaseDate, src => src.Album.ReleaseDate);
        }

        private static void ConfigureFavoriteBandMappings()
        {
            TypeAdapterConfig<FavoriteBand, FavoriteBandDto>.NewConfig()
                .Map(dest => dest.BandName, src => src.Band.BandName)
                .Map(dest => dest.LogoUrl, src => src.Band.LogoUrl)
                .Map(dest => dest.ReleaseDate, src => src.Band.ReleaseDate);
        }
    }
}
