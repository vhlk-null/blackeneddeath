namespace Library.Application.Albums.Commands.CreateAlbum;

public class CreateAlbumHandler(ILibraryDbContext context) : BuildingBlocks.CQRS.ICommandHandler<CreateAlbumCommand, CreateAlbumResult>
{
    public async ValueTask<CreateAlbumResult> Handle(CreateAlbumCommand command, CancellationToken cancellationToken)
    {
        var album = CreateNewAlbum(command.Album);
        context.Albums.Add(album);
        await context.SaveChangesAsync(cancellationToken);

        return new CreateAlbumResult(album.Id.Value);
    }

    private static Album CreateNewAlbum(AlbumDto album)
    {
        var albumRelease = AlbumRelease.Of(album.ReleaseDate, album.Format);
        var labelInfo = string.IsNullOrWhiteSpace(album.Label) ? null : LabelInfo.Of(album.Label);

        var newAlbum = Album.Create(album.Title, album.Type, albumRelease, album.CoverUrl, labelInfo);

        foreach (var band in album.Bands)
            newAlbum.AddBand(BandId.Of(band.Id));

        foreach (var country in album.Countries)
            newAlbum.AddCountry(CountryId.Of(country.Id));

        foreach (var genre in album.Genres)
            newAlbum.AddGenre(GenreId.Of(genre.Id), isPrimary: genre.IsPrimary);

        foreach (var link in album.StreamingLinks)
            newAlbum.AddStreamingLink(link.Platform, link.EmbedCode);

        foreach (var track in album.Tracks)
            newAlbum.AddTrack(TrackId.Of(track.Id), track.TrackNumber);

        return newAlbum;
    }
}