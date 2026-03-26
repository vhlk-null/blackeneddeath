namespace Library.Application.Services.Albums.Commands.CreateAlbum;

public class CreateAlbumHandler(ILibraryDbContext context, IStorageService storage) : BuildingBlocks.CQRS.ICommandHandler<CreateAlbumCommand, CreateAlbumResult>
{
    public async ValueTask<CreateAlbumResult> Handle(CreateAlbumCommand command, CancellationToken cancellationToken)
    {
        await ValidateReferencedEntitiesAsync(command.Album, cancellationToken);

        string? coverKey = null;
        if (command.CoverImage is not null && command.CoverImageContentType is not null && command.CoverImageFileName is not null)
        {
            var extension = Path.GetExtension(command.CoverImageFileName);
            coverKey = await storage.UploadFileAsync("album-images", $"{Guid.NewGuid()}{extension}", command.CoverImage, command.CoverImageContentType, cancellationToken);
        }

        var album = CreateNewAlbum(command.Album with { CoverUrl = coverKey });

        var tracks = command.Album.Tracks
            .Select(t => Track.Create(TrackId.Of(Guid.NewGuid()), t.Title))
            .ToList();

        await context.Tracks.AddRangeAsync(tracks, cancellationToken);

        foreach (var (track, dto) in tracks.Zip(command.Album.Tracks))
            album.AddTrack(track.Id, dto.TrackNumber);

        context.Albums.Add(album);
        await context.SaveChangesAsync(cancellationToken);

        return new CreateAlbumResult(album.Id.Value);
    }

    private async Task ValidateReferencedEntitiesAsync(AlbumDto album, CancellationToken cancellationToken)
    {
        foreach (var band in album.Bands)
        {
            if (!band.Id.HasValue || band.Id.Value == Guid.Empty) continue;

            var bandId = BandId.Of(band.Id.Value);
            if (!await context.Bands.AnyAsync(b => b.Id == bandId, cancellationToken))
                throw new BandNotFoundException(band.Id.Value);
        }

        foreach (var country in album.Countries)
        {
            var countryId = CountryId.Of(country.Id);
            if (!await context.Countries.AnyAsync(c => c.Id == countryId, cancellationToken))
                throw new CountryNotFoundException(country.Id);
        }

        foreach (var genre in album.Genres)
        {
            var genreId = GenreId.Of(genre.Id);
            if (!await context.Genres.AnyAsync(g => g.Id == genreId, cancellationToken))
                throw new GenreNotFoundException(genre.Id);
        }

        if (album.Label?.Id is Guid labelId && labelId != Guid.Empty)
        {
            var lid = LabelId.Of(labelId);
            if (!await context.Labels.AnyAsync(l => l.Id == lid, cancellationToken))
                throw new LabelNotFoundException(labelId);
        }
    }

    private Album CreateNewAlbum(AlbumDto album)
    {
        var albumRelease = AlbumRelease.Of(album.ReleaseDate, album.Format);
        var labelId = album.Label?.Id is Guid lid && lid != Guid.Empty ? LabelId.Of(lid) : null;

        var newAlbum = Album.Create(album.Title, album.Type, albumRelease, album.CoverUrl, labelId);

        foreach (var band in album.Bands)
        {
            if (band.Id is not null && band.Id != Guid.Empty)
            {
                newAlbum.AddBand(BandId.Of(band.Id.Value));
            }
            else
            {
                var newBand = Band.Create(band.Name, null, null, BandActivity.Of(null, null), BandStatus.Unknown);
                context.Bands.Add(newBand);
                newAlbum.AddBand(newBand.Id);
            }
        }

        foreach (var country in album.Countries)
            newAlbum.AddCountry(CountryId.Of(country.Id));

        foreach (var genre in album.Genres)
            newAlbum.AddGenre(GenreId.Of(genre.Id), isPrimary: genre.IsPrimary);

        foreach (var link in album.StreamingLinks)
            newAlbum.AddStreamingLink(link.Platform, link.EmbedCode);

        return newAlbum;
    }
}