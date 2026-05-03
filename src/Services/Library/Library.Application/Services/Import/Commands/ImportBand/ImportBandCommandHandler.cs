using Library.Application.Services.Import;

namespace Library.Application.Services.Import.Commands.ImportBand;

public class ImportBandCommandHandler(
    IBandImportService musicBrainz,
    ILibraryDbContext context,
    ImportStatusService importStatus,
    ILogger<ImportBandCommandHandler> logger)
    : BuildingBlocks.CQRS.ICommandHandler<ImportBandCommand, ImportBandResult>
{
    public async ValueTask<ImportBandResult> Handle(ImportBandCommand command, CancellationToken cancellationToken)
    {
        bool bandAlreadyExists = await context.Bands
            .AnyAsync(b => b.Name.ToLower() == command.BandName.ToLower().Trim(), cancellationToken);

        bool hasSelectedAlbums = command.SelectedAlbumMbIds is { Count: > 0 };

        if (bandAlreadyExists && !hasSelectedAlbums)
            throw new BadRequestException($"Band '{command.BandName}' already exists in the database.");

        CancellationToken importCt = importStatus.Start(command.BandName);

        var progress = new CompositeProgress(command.Progress, importStatus);

        MusicBrainzImportResult result = await musicBrainz.ImportByIdAsync(
            command.MbId, command.BandName, command.SelectedAlbumMbIds, progress, importCt);

        if (!result.Success || result.Band is null)
        {
            importStatus.Fail(result.ErrorMessage ?? $"Could not import '{command.BandName}'");
            throw new BadRequestException(result.ErrorMessage ?? $"Could not import '{command.BandName}' from MusicBrainz.");
        }

        Band band = await ResolveOrCreateBandAsync(result.Band, cancellationToken);

        CountryId? bandCountryId = band.BandCountries.FirstOrDefault()?.CountryId;

        int albumsImported = 0;
        int albumsSkipped = 0;
        HashSet<string> usedSlugs = [];

        foreach (AlbumImportData albumData in result.Albums)
        {
            string baseSlug = $"{SlugHelper.Generate(albumData.Title)}-{albumData.ReleaseYear}";

            if (albumData.ReleaseYear <= 0)
            {
                logger.LogWarning("Skipping '{Title}' — no release year", albumData.Title);
                albumsSkipped++;
                continue;
            }

            bool exists = await context.Albums.AnyAsync(a => a.Slug == baseSlug, cancellationToken);
            if (exists || usedSlugs.Contains(baseSlug))
            {
                albumsSkipped++;
                continue;
            }

            string slug = await GenerateUniqueSlugAsync(albumData.Title, albumData.ReleaseYear, usedSlugs, cancellationToken);
            usedSlugs.Add(slug);

            (Album album, List<Track> tracks) = MapAlbum(albumData, band.Id, slug, labelId: null);

            if (bandCountryId is not null)
                album.AddCountry(bandCountryId);

            await context.Tracks.AddRangeAsync(tracks, cancellationToken);

            foreach ((Track track, TrackImportData dto) in tracks.Zip(albumData.Tracks))
                album.AddTrack(track.Id, dto.TrackNumber);

            foreach (StreamingLinkImportData link in albumData.StreamingLinks)
                album.AddStreamingLink(link.Platform, link.Url);

            logger.LogInformation("  '{Title}' — genres: [{Genres}], label: '{Label}'",
                albumData.Title,
                string.Join(", ", albumData.Genres),
                albumData.LabelName ?? "none");

            bool anyGenreAdded = false;
            foreach (string genreName in albumData.Genres)
            {
                Genre? genre = await context.Genres
                    .FirstOrDefaultAsync(g => g.Name.ToLower() == genreName.ToLower(), cancellationToken);
                if (genre is null)
                {
                    logger.LogWarning("  Genre '{Genre}' not found in DB — skipping", genreName);
                    continue;
                }
                album.AddGenre(genre.Id, isPrimary: !anyGenreAdded);
                anyGenreAdded = true;
                logger.LogInformation("  Genre '{Genre}' assigned", genreName);
            }

            if (albumData.LabelName is not null)
            {
                Label? label = await context.Labels
                    .FirstOrDefaultAsync(l => l.Name.ToLower() == albumData.LabelName.ToLower(), cancellationToken);
                if (label is null)
                {
                    label = Label.Create(LabelId.Of(Guid.NewGuid()), albumData.LabelName);
                    context.Labels.Add(label);
                    logger.LogInformation("  Label '{Label}' created", albumData.LabelName);
                }
                else
                {
                    logger.LogInformation("  Label '{Label}' found and assigned", albumData.LabelName);
                }
                album.AssignLabel(label.Id);
            }

            context.Albums.Add(album);
            albumsImported++;
        }

        await context.SaveChangesAsync(importCt);

        logger.LogInformation("Imported band '{Name}': {Imported} albums saved, {Skipped} skipped",
            band.Name, albumsImported, albumsSkipped);

        var notices = new List<string>();
        string doneMessage = $"Done! {band.Name}: {albumsImported} albums imported, {albumsSkipped} skipped";

        importStatus.Complete(doneMessage);
        command.Progress?.Report(new ImportProgressEvent(ImportProgressStage.Done, doneMessage));

        return new ImportBandResult(band.Id.Value, band.Name, albumsImported, albumsSkipped, []);
    }

    private async Task<Band> ResolveOrCreateBandAsync(BandImportData data, CancellationToken ct)
    {
        Band? existing = await context.Bands
            .Include(b => b.BandCountries)
            .Include(b => b.BandGenres)
            .FirstOrDefaultAsync(b => b.Name.ToLower() == data.Name.ToLower().Trim(), ct);

        if (existing is not null)
            return existing;

        BandActivity activity = BandActivity.Of(data.FormedYear, data.DisbandedYear);
        BandStatus status = data.IsActive ? BandStatus.Active : BandStatus.Disbanded;

        Band band = Band.Create(data.Name, bio: null, logoUrl: null, activity, status);

        if (data.Country is not null)
        {
            Country? country = await context.Countries
                .FirstOrDefaultAsync(c => c.Code != null && c.Code.ToLower() == data.Country.ToLower(), ct);

            if (country is not null)
                band.AddCountry(country.Id);
        }

        logger.LogInformation("MusicBrainz tags for '{Name}': {Tags}", data.Name, string.Join(", ", data.Tags));

        bool anyGenreAdded = false;
        foreach ((string tag, int index) in data.Tags.Select((t, i) => (t, i)))
        {
            Genre? genre = await context.Genres
                .FirstOrDefaultAsync(g => g.Name.ToLower() == tag.ToLower(), ct);

            if (genre is null)
            {
                logger.LogWarning("Genre not found in DB: '{Tag}', skipping", tag);
                continue;
            }

            band.AddGenre(genre.Id, isPrimary: !anyGenreAdded);
            anyGenreAdded = true;
        }

        if (!anyGenreAdded)
        {
            Genre? defaultGenre = await context.Genres
                .FirstOrDefaultAsync(g => g.Name.ToLower() == "black death metal", ct);

            if (defaultGenre is not null)
            {
                band.AddGenre(defaultGenre.Id, isPrimary: true);
                logger.LogInformation("No matching genres found for '{Name}', assigned default 'Black Death Metal'", data.Name);
            }
            else
            {
                logger.LogWarning("Default genre 'Black Death Metal' not found in DB either");
            }
        }

        context.Bands.Add(band);
        return band;
    }

    private async Task<string> GenerateUniqueSlugAsync(string title, int year, HashSet<string> usedSlugs, CancellationToken ct)
    {
        string baseSlug = $"{SlugHelper.Generate(title)}-{year}";
        string slug = baseSlug;
        int counter = 1;

        while (usedSlugs.Contains(slug) || await context.Albums.AnyAsync(a => a.Slug == slug, ct))
            slug = $"{baseSlug}-{++counter}";

        return slug;
    }


    private static (Album album, List<Track> tracks) MapAlbum(AlbumImportData data, BandId bandId, string slug, LabelId? labelId)
    {
        AlbumRelease release = AlbumRelease.Of(data.ReleaseYear, AlbumFormat.CD, data.ReleaseMonth, data.ReleaseDay);
        AlbumType type = MapAlbumType(data.TypeHint);

        Album album = Album.Create(data.Title, type, release, coverUrl: data.CoverUrl, labelId: labelId, slug: slug);
        album.AddBand(bandId);

        List<Track> tracks = data.Tracks
            .Select(t => Track.Create(TrackId.Of(Guid.NewGuid()), t.Title, t.Duration))
            .ToList();

        return (album, tracks);
    }

    private sealed class CompositeProgress(
        IProgress<ImportProgressEvent>? inner,
        ImportStatusService status) : IProgress<ImportProgressEvent>
    {
        public void Report(ImportProgressEvent value)
        {
            inner?.Report(value);
            status.Update(value.Message, value.Current, value.Total);
        }
    }

    private static AlbumType MapAlbumType(AlbumTypeHint hint) => hint switch
    {
        AlbumTypeHint.FullLength  => AlbumType.FullLength,
        AlbumTypeHint.EP          => AlbumType.EP,
        AlbumTypeHint.Single      => AlbumType.Single,
        AlbumTypeHint.Demo        => AlbumType.Demo,
        AlbumTypeHint.LiveAlbum   => AlbumType.LiveAlbum,
        AlbumTypeHint.Compilation => AlbumType.Compilation,
        AlbumTypeHint.Split       => AlbumType.Split,
        _                         => AlbumType.FullLength
    };
}
