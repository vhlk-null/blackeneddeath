namespace Library.Application.Services.Albums.Queries.GetAlbumById;

public class GetAlbumByIdQueryHandler(ILibraryDbContext context, IStorageUrlResolver urlResolver) : BuildingBlocks.CQRS.IQueryHandler<GetAlbumByIdQuery, GetAlbumByIdResult>
{
    public async ValueTask<GetAlbumByIdResult> Handle(GetAlbumByIdQuery query, CancellationToken cancellationToken)
    {
        var albumId = AlbumId.Of(query.Id);

        var album = await context.Albums
            .AsNoTracking()
            .Include(a => a.AlbumBands)
            .Include(a => a.AlbumGenres)
            .Include(a => a.AlbumCountries)
            .Include(a => a.AlbumTracks)
            .Include(a => a.AlbumTags)
            .Include(a => a.StreamingLinks)
            .FirstOrDefaultAsync(a => a.Id == albumId, cancellationToken)
            ?? throw new AlbumNotFoundException(query.Id);

        var bands = await context.Bands.AsNoTracking()
            .Where(b => album.AlbumBands.Select(ab => ab.BandId).Contains(b.Id))
            .ToDictionaryAsync(b => b.Id, cancellationToken);

        var genres = await context.Genres.AsNoTracking()
            .Where(g => album.AlbumGenres.Select(ag => ag.GenreId).Contains(g.Id))
            .ToDictionaryAsync(g => g.Id, cancellationToken);

        var countries = await context.Countries.AsNoTracking()
            .Where(c => album.AlbumCountries.Select(ac => ac.CountryId).Contains(c.Id))
            .ToDictionaryAsync(c => c.Id, cancellationToken);

        var tracks = await context.Tracks.AsNoTracking()
            .Where(t => album.AlbumTracks.Select(at => at.TrackId).Contains(t.Id))
            .ToDictionaryAsync(t => t.Id, cancellationToken);

        var labels = album.LabelId != null
            ? await context.Labels.AsNoTracking()
                .Where(l => l.Id == album.LabelId)
                .ToDictionaryAsync(l => l.Id, cancellationToken)
            : new Dictionary<LabelId, Label>();

        var tagIds = album.AlbumTags.Select(at => at.TagId).ToList();
        var tags = tagIds.Count > 0
            ? await context.Tags.AsNoTracking()
                .Where(t => tagIds.Contains(t.Id))
                .ToDictionaryAsync(t => t.Id, cancellationToken)
            : new Dictionary<TagId, Tag>();

        return new GetAlbumByIdResult(album.ToAlbumDto(bands, genres, countries, tracks, urlResolver, labels, tags));
    }
}
