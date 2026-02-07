using Archive.API.Models;
using AlbumDtos = Archive.API.Albums.GetAlbums;
using BandDtos = Archive.API.Bands.GetBands;

namespace Archive.API.Mappings
{
    public static class MappingConfig
    {
        public static void RegisterMappings()
        {
            ConfigureAlbumMappings();
            ConfigureBandMappings();
        }

        private static void ConfigureAlbumMappings()
        {
            TypeAdapterConfig<Album, AlbumDtos.AlbumDto>.NewConfig()
                .Map(dest => dest.Bands,
                     src => src.Bands.Select(ab => new AlbumDtos.BandDto(ab.Band.Id, ab.Band.Name, ab.Band.LogoUrl)))
                .Map(dest => dest.Countries,
                     src => src.Countries.Select(ac => new AlbumDtos.CountryDto(ac.Country.Id, ac.Country.Name, ac.Country.Code)))
                .Map(dest => dest.Tracks,
                     src => src.Tracks.Select(at => new AlbumDtos.TrackDto(at.Track.Id, at.Track.Title)))
                .Map(dest => dest.Genres,
                     src => src.Genres.Select(ag => new AlbumDtos.GenreDto(ag.Genre.Id, ag.Genre.Name)))
                .Map(dest => dest.StreamingLinks,
                     src => src.StreamingLinks.Select(sl => sl.EmbedCode ?? ""));
        }

        private static void ConfigureBandMappings()
        {
            TypeAdapterConfig<Band, BandDtos.BandDto>.NewConfig()
                .Map(dest => dest.Country,
                     src => src.Country == null ? null : new BandDtos.CountryDto(src.Country.Id, src.Country.Name, src.Country.Code))
                .Map(dest => dest.Albums,
                     src => src.Albums.Select(ab => new BandDtos.AlbumDto(ab.Album.Id, ab.Album.Title, ab.Album.ReleaseDate)))
                .Map(dest => dest.Genres,
                     src => src.Genres.Select(bg => new BandDtos.GenreDto(bg.Genre.Id, bg.Genre.Name)));
        }
    }
}
