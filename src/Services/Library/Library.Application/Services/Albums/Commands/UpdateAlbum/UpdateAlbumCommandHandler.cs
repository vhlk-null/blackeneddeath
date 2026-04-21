namespace Library.Application.Services.Albums.Commands.UpdateAlbum;

public class UpdateAlbumCommandHandler(ILibraryDbContext context, IStorageService storage) : BuildingBlocks.CQRS.ICommandHandler<UpdateAlbumCommand, UpdateAlbumResult>
{
    public async ValueTask<UpdateAlbumResult> Handle(UpdateAlbumCommand command, CancellationToken cancellationToken)
    {
        AlbumId albumId = AlbumId.Of(command.Album.Id);

        Album album = await context.Albums
                          .Include(a => a.AlbumBands)
                          .Include(a => a.AlbumGenres)
                          .Include(a => a.AlbumCountries)
                          .Include(a => a.StreamingLinks)
                          .Include(a => a.AlbumTracks)
                          .Include(a => a.AlbumTags)
                          .FirstOrDefaultAsync(a => a.Id == albumId, cancellationToken)
                      ?? throw new AlbumNotFoundException(command.Album.Id);

        List<TrackId> albumTrackIds = album.AlbumTracks.Select(x => x.TrackId).ToList();
        List<Track> albumTracks = await context.Tracks
            .Where(t => albumTrackIds.Contains(t.Id))
            .ToListAsync(cancellationToken);

        if (command.Album.LabelIds is { Count: > 0 } && command.Album.LabelNames is not { Count: > 0 })
        {
            Guid labelGuid = command.Album.LabelIds[0];
            if (!await context.Labels.AnyAsync(l => l.Id == LabelId.Of(labelGuid), cancellationToken))
                throw new LabelNotFoundException(labelGuid);
        }

        string? coverKey = album.CoverUrl;
        if (command.CoverImage is not null && command.CoverImageContentType is not null && command.CoverImageFileName is not null)
        {
            if (album.CoverUrl is not null)
                await storage.DeleteFileAsync(album.CoverUrl, cancellationToken);

            string folder = $"albums/{Slugify(command.Album.Title)}";
            string extension = Path.GetExtension(command.CoverImageFileName);
            coverKey = await storage.UploadFileAsync(folder, $"{Guid.NewGuid()}{extension}", command.CoverImage, command.CoverImageContentType, cancellationToken);
        }

        AlbumRelease albumRelease = AlbumRelease.Of(command.Album.ReleaseDate, command.Album.Format, command.Album.ReleaseMonth, command.Album.ReleaseDay);
        LabelId? labelId = await ResolveLabelAsync(command.Album, cancellationToken);

        string newSlug = album.Title != command.Album.Title
            ? await GenerateUniqueSlugAsync(command.Album.Title, command.Album.ReleaseDate, album.Id, cancellationToken)
            : album.Slug;

        album.Update(command.Album.Title, newSlug, command.Album.Type, albumRelease, coverKey, labelId, command.Album.IsExplicit);
        album.Approve();

        List<BandId> bandIds = await ResolveBandIdsAsync(command.Album, cancellationToken);
        ReconcileBands(album, bandIds);
        ReconcileCountries(album, command.Album);
        ReconcileGenres(album, command.Album);
        ReconcileTags(album, command.Album);
        ReconcileStreamingLinks(album, command.Album);
        await ReconcileTracksAsync(album, albumTracks, command.Album, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);

        return new UpdateAlbumResult(true);
    }

    private async Task<string> GenerateUniqueSlugAsync(string title, int releaseYear, AlbumId excludeId, CancellationToken ct)
    {
        string baseSlug = $"{SlugHelper.Generate(title)}-{releaseYear}";
        string slug = baseSlug;
        int counter = 1;
        while (await context.Albums.AnyAsync(a => a.Slug == slug && a.Id != excludeId, ct))
            slug = $"{baseSlug}-{++counter}";
        return slug;
    }

    private async Task<List<BandId>> ResolveBandIdsAsync(UpdateAlbumDto album, CancellationToken cancellationToken)
    {
        List<BandId> result = album.BandIds.Select(BandId.Of).ToList();

        if (album.BandNames is not { Count: > 0 })
            return result;

        foreach (string name in album.BandNames)
        {
            string trimmed = name.Trim();
            Band? existing = await context.Bands.FirstOrDefaultAsync(b => b.Name.ToLower() == trimmed.ToLower(), cancellationToken);
            if (existing is not null)
            {
                result.Add(existing.Id);
                continue;
            }

            Band newBand = Band.Create(trimmed, null, null, BandActivity.Of(null, null), BandStatus.Unknown);
            await context.Bands.AddAsync(newBand, cancellationToken);
            result.Add(newBand.Id);
        }

        return result;
    }

    private static void ReconcileBands(Album album, List<BandId> incomingIds)
    {
        HashSet<BandId> currentIds = album.AlbumBands.Select(x => x.BandId).ToHashSet();
        HashSet<BandId> incoming = incomingIds.ToHashSet();

        foreach (BandId id in currentIds.Except(incoming))
            album.RemoveBand(id);

        foreach (BandId id in incoming.Except(currentIds))
            album.AddBand(id);

        album.ReorderBands(incomingIds.Where(id => album.AlbumBands.Any(ab => ab.BandId == id)).ToList());
    }

    private static void ReconcileCountries(Album album, UpdateAlbumDto dto)
    {
        HashSet<CountryId> currentIds = album.AlbumCountries.Select(x => x.CountryId).ToHashSet();
        HashSet<CountryId> incomingIds = dto.CountryIds.Select(CountryId.Of).ToHashSet();

        foreach (CountryId id in currentIds.Except(incomingIds))
            album.RemoveCountry(id);

        foreach (CountryId id in incomingIds.Except(currentIds))
            album.AddCountry(id);
    }

    private static void ReconcileGenres(Album album, UpdateAlbumDto dto)
    {
        HashSet<GenreId> currentIds = album.AlbumGenres.Select(x => x.GenreId).ToHashSet();
        List<GenreId> incomingOrdered = dto.GenreIds.Select(GenreId.Of).ToList();
        HashSet<GenreId> incomingIds = incomingOrdered.ToHashSet();

        foreach (GenreId id in currentIds.Except(incomingIds))
            album.RemoveGenre(id);

        foreach ((GenreId genreId, int index) in incomingOrdered.Select((id, i) => (id, i)))
        {
            if (currentIds.Contains(genreId))
                album.UpdateGenrePrimary(genreId, isPrimary: index == 0);
            else
                album.AddGenre(genreId, isPrimary: index == 0);
        }
    }

    private static void ReconcileTags(Album album, UpdateAlbumDto dto)
    {
        HashSet<TagId> currentIds = album.AlbumTags.Select(x => x.TagId).ToHashSet();
        HashSet<TagId> incomingIds = dto.TagIds.Select(TagId.Of).ToHashSet();

        foreach (TagId id in currentIds.Except(incomingIds))
            album.RemoveTag(id);

        foreach (TagId id in incomingIds.Except(currentIds))
            album.AddTag(id);
    }

    private static void ReconcileStreamingLinks(Album album, UpdateAlbumDto dto)
    {
        Dictionary<StreamingPlatform, StreamingLinkDto> incomingByPlatform = dto.StreamingLinks.ToDictionary(x => x.Platform);

        foreach (StreamingLink link in album.StreamingLinks.ToList())
        {
            if (!incomingByPlatform.ContainsKey(link.Platform))
                album.RemoveStreamingLink(link.Id);
        }

        Dictionary<StreamingPlatform, StreamingLink> currentByPlatform = album.StreamingLinks.ToDictionary(x => x.Platform);

        foreach (StreamingLinkDto incoming in dto.StreamingLinks)
        {
            if (currentByPlatform.TryGetValue(incoming.Platform, out StreamingLink? existing))
            {
                if (existing.EmbedCode != incoming.EmbedCode)
                    album.UpdateStreamingLink(existing.Id, incoming.EmbedCode);
            }
            else
            {
                album.AddStreamingLink(incoming.Platform, incoming.EmbedCode);
            }
        }
    }

    private async Task ReconcileTracksAsync(Album album, List<Track> currentTracks, UpdateAlbumDto dto, CancellationToken cancellationToken)
    {
        if (dto.Tracks is null or { Count: 0 })
            return;

        Dictionary<int, TrackInputDto> incomingByNumber = dto.Tracks.ToDictionary(t => t.TrackNumber);
        Dictionary<TrackId, Track> tracksById       = currentTracks.ToDictionary(t => t.Id);
        Dictionary<int, Track> currentByNumber  = album.AlbumTracks
            .Where(at => tracksById.ContainsKey(at.TrackId))
            .ToDictionary(at => at.TrackNumber, at => tracksById[at.TrackId]);

        // Remove tracks no longer in the list
        foreach ((int number, Track track) in currentByNumber)
        {
            if (!incomingByNumber.ContainsKey(number))
            {
                album.RemoveTrack(track.Id);
                context.Tracks.Remove(track);
            }
        }

        // Update existing or add new
        foreach (TrackInputDto incoming in dto.Tracks)
        {
            if (currentByNumber.TryGetValue(incoming.TrackNumber, out Track? existing))
            {
                if (existing.Title != incoming.Title)
                    existing.UpdateTitle(incoming.Title);

                if (existing.Duration != incoming.Duration) 
                    existing.UpdateDuration(incoming.Duration);
            }
            else
            {
                Track newTrack = Track.Create(TrackId.Of(Guid.NewGuid()), incoming.Title, incoming.Duration);
                await context.Tracks.AddAsync(newTrack, cancellationToken);
                album.AddTrack(newTrack.Id, incoming.TrackNumber);
            }
        }
    }

    private async Task<LabelId?> ResolveLabelAsync(UpdateAlbumDto album, CancellationToken cancellationToken)
    {
        if (album.LabelNames is { Count: > 0 })
        {
            string name = album.LabelNames[0].Trim();
            Label? existing = await context.Labels.FirstOrDefaultAsync(l => l.Name.ToLower() == name.ToLower(), cancellationToken);
            if (existing is not null)
                return existing.Id;

            Label newLabel = Label.Create(LabelId.Of(Guid.NewGuid()), name);
            await context.Labels.AddAsync(newLabel, cancellationToken);
            return newLabel.Id;
        }

        return album.LabelIds is { Count: > 0 } ? LabelId.Of(album.LabelIds[0]) : null;
    }

    private static string Slugify(string value) =>
        Regex.Replace(value.ToLowerInvariant().Trim(), @"[^a-z0-9]+", "-").Trim('-');
}
