using Archive.API.Albums.GetAlbums;
using Archive.API.Models;

namespace Archive.API.Mappings
{
    public static class AlbumMappingConfig
    {
        public static void ConfigureAlbumMappings()
        {
            TypeAdapterConfig<Album, AlbumDto>.NewConfig()
                .Map(dest => dest.Bands,
                     src => src.Bands.Select(ab => new BandDto(ab.Band.Id, ab.Band.Name, ab.Band.LogoUrl)))
                .Map(dest => dest.Countries,
                     src => src.Countries.Select(ac => new CountryDto(ac.Country.Id, ac.Country.Name, ac.Country.Code)))
                .Map(dest => dest.Tracks,
                     src => src.Tracks.Select(at => new TrackDto(at.Track.Id, at.Track.Title)))
                .Map(dest => dest.Genres,
                     src => src.Genres.Select(ag => new GenreDto(ag.Genre.Id, ag.Genre.Name)))
                .Map(dest => dest.StreamingLinks,
                     src => src.StreamingLinks.Select(sl => sl.EmbedCode ?? ""));
        }
    }
}
