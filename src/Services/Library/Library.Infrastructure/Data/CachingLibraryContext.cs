using Library.Application.Data;
using Library.Domain.Models;
using Library.Domain.Models.JoinTables;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Library.Infrastructure.Data;

public class CachingLibraryContext(
    ILibraryDbContext inner,
    IDistributedCache cache,
    ILogger<CachingLibraryContext> logger)
    : ILibraryDbContext
{
    private static readonly DistributedCacheEntryOptions StaticOptions =
        new() { AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(30) };

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        TypeInfoResolver = new System.Text.Json.Serialization.Metadata.DefaultJsonTypeInfoResolver
        {
            Modifiers =
            {
                static ti =>
                {
                    if (ti.CreateObject is null && ti.Type.GetConstructor(
                            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance,
                            Type.EmptyTypes) is { } ctor)
                        ti.CreateObject = () => ctor.Invoke(null);
                }
            }
        }
    };

    // ── Cache keys ────────────────────────────────────────────────────────────

    private const string GenresKey = "genre:all";
    private const string CountriesKey = "countries:all";
    private const string LabelsKey = "labels:all";
    private const string TagsKey = "tags:all";
    private const string GenreCardsKey = "genrecards:all";

    // ── DbSet pass-through ────────────────────────────────────────────────────

    public DbSet<Album> Albums => inner.Albums;
    public DbSet<Band> Bands => inner.Bands;
    public DbSet<Track> Tracks => inner.Tracks;
    public DbSet<Genre> Genres => inner.Genres;
    public DbSet<Country> Countries => inner.Countries;
    public DbSet<Label> Labels => inner.Labels;
    public DbSet<Tag> Tags => inner.Tags;
    public DbSet<StreamingLink> StreamingLinks => inner.StreamingLinks;
    public DbSet<GenreCard> GenreCards => inner.GenreCards;
    public DbSet<VideoBand> VideoBands => inner.VideoBands;

    public DbSet<AlbumBand> AlbumBands => inner.AlbumBands;
    public DbSet<AlbumGenre> AlbumGenres => inner.AlbumGenres;
    public DbSet<AlbumTrack> AlbumTracks => inner.AlbumTracks;
    public DbSet<AlbumTag> AlbumTags => inner.AlbumTags;
    public DbSet<BandGenre> BandGenres => inner.BandGenres;
    public DbSet<BandCountry> BandCountries => inner.BandCountries;
    public DbSet<AlbumCountry> AlbumCountries => inner.AlbumCountries;
    public DbSet<GenreCardGenre> GenreCardGenres => inner.GenreCardGenres;
    public DbSet<GenreCardTag> GenreCardTags => inner.GenreCardTags;

    public DbSet<AlbumRating> AlbumRatings => inner.AlbumRatings;
    public DbSet<BandRating> BandRatings => inner.BandRatings;

    // ── Cached reads ──────────────────────────────────────────────────────────

    public Task<List<Genre>> GetAllGenresAsync(CancellationToken cancellationToken = default) =>
        GetOrSetAsync(GenresKey, ct => inner.GetAllGenresAsync(ct), cancellationToken);

    public Task<List<Country>> GetAllCountriesAsync(CancellationToken cancellationToken = default) =>
        GetOrSetAsync(CountriesKey, ct => inner.GetAllCountriesAsync(ct), cancellationToken);

    public Task<List<Label>> GetAllLabelsAsync(CancellationToken cancellationToken = default) =>
        GetOrSetAsync(LabelsKey, ct => inner.GetAllLabelsAsync(ct), cancellationToken);

    public Task<List<Tag>> GetAllTagsAsync(CancellationToken cancellationToken = default) =>
        GetOrSetAsync(TagsKey, ct => inner.GetAllTagsAsync(ct), cancellationToken);

    public Task<List<GenreCard>> GetAllGenreCardsAsync(CancellationToken cancellationToken = default) =>
        GetOrSetAsync(GenreCardsKey, ct => inner.GetAllGenreCardsAsync(ct), cancellationToken);

    public Task<Album?> GetAlbumBySlugAsync(string slug, bool approvedOnly, CancellationToken cancellationToken = default) =>
        inner.GetAlbumBySlugAsync(slug, approvedOnly, cancellationToken);

    // ── SaveChanges with automatic cache invalidation ─────────────────────────

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        var entries = ((LibraryContext)inner).ChangeTracker.Entries()
            .Where(e => e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted)
            .ToList();

        var keysToRemove = new HashSet<string>();

        foreach (var entry in entries)
        {
            switch (entry.Entity)
            {
                case Genre:
                    keysToRemove.Add(GenresKey);
                    break;

                case Label:
                    keysToRemove.Add(LabelsKey);
                    break;

                case Country:
                    keysToRemove.Add(CountriesKey);
                    break;

                case Tag:
                    keysToRemove.Add(TagsKey);
                    break;

                case GenreCard:
                    keysToRemove.Add(GenreCardsKey);
                    break;
            }
        }

        int result = await inner.SaveChangesAsync(cancellationToken);

        if (keysToRemove.Count > 0)
            await Task.WhenAll(keysToRemove.Select(key => RemoveAsync(key, cancellationToken)));

        return result;
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private async Task<TResult> GetOrSetAsync<TResult>(
        string key,
        Func<CancellationToken, Task<TResult>> factory,
        CancellationToken ct)
    {
        try
        {
            byte[]? bytes = await cache.GetAsync(key, ct);
            if (bytes is not null)
            {
                logger.LogDebug("Cache HIT  [{Key}]", key);
                return JsonSerializer.Deserialize<TResult>(bytes, JsonOptions)!;
            }
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Cache GET error [{Key}]", key);
        }

        logger.LogDebug("Cache MISS [{Key}]", key);
        TResult result = await factory(ct);

        if (result is not null)
        {
            try
            {
                byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(result, JsonOptions);
                await cache.SetAsync(key, bytes, StaticOptions, ct);
                logger.LogDebug("Cache SET  [{Key}]", key);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Cache SET error [{Key}]", key);
            }
        }

        return result!;
    }

    private async Task RemoveAsync(string key, CancellationToken ct)
    {
        try
        {
            await cache.RemoveAsync(key, ct);
            logger.LogDebug("Cache DEL [{Key}]", key);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Cache DEL error [{Key}]", key);
        }
    }
}
