namespace UserContent.Application.Consumers;

public class AlbumUpdatedConsumer(IRepository<UserContentContext> repo) : IConsumer<AlbumUpdatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<AlbumUpdatedIntegrationEvent> consumeContext)
    {
        Album? album = await repo.GetByAsync<Album>(
            a => a.Id == consumeContext.Message.AlbumId,
            cancellationToken: consumeContext.CancellationToken);

        if (album is null)
        {
            album = new Album
            {
                Id = consumeContext.Message.AlbumId,
                Title = consumeContext.Message.Title,
                Slug = consumeContext.Message.Slug,
                CoverUrl = consumeContext.Message.CoverUrl,
                ReleaseDate = consumeContext.Message.ReleaseDate,
                Format = consumeContext.Message.Format,
                Type = consumeContext.Message.Type,
                PrimaryGenreName = consumeContext.Message.PrimaryGenre?.Name,
                PrimaryGenreSlug = consumeContext.Message.PrimaryGenre?.Slug,
                BandNames = consumeContext.Message.Bands.Count > 0
                    ? string.Join(",", consumeContext.Message.Bands.Select(b => b.Name))
                    : null,
                BandSlugs = consumeContext.Message.Bands.Count > 0
                    ? string.Join(",", consumeContext.Message.Bands.Select(b => b.Slug ?? ""))
                    : null,
                CountryNames = consumeContext.Message.Countries.Count > 0
                    ? string.Join(",", consumeContext.Message.Countries.Select(c => c.Name))
                    : null,
                CountryCodes = consumeContext.Message.Countries.Count > 0
                    ? string.Join(",", consumeContext.Message.Countries.Select(c => c.Code ?? ""))
                    : null
            };
            await repo.AddAsync(album, consumeContext.CancellationToken);
            await repo.SaveChangesAsync(consumeContext.CancellationToken);
            return;
        }

        album.Title = consumeContext.Message.Title;
        album.Slug = consumeContext.Message.Slug;
        album.CoverUrl = consumeContext.Message.CoverUrl;
        album.ReleaseDate = consumeContext.Message.ReleaseDate;
        album.Format = consumeContext.Message.Format;
        album.Type = consumeContext.Message.Type;
        album.PrimaryGenreName = consumeContext.Message.PrimaryGenre?.Name;
        album.PrimaryGenreSlug = consumeContext.Message.PrimaryGenre?.Slug;
        album.BandNames = consumeContext.Message.Bands.Count > 0
            ? string.Join(",", consumeContext.Message.Bands.Select(b => b.Name))
            : null;
        album.BandSlugs = consumeContext.Message.Bands.Count > 0
            ? string.Join(",", consumeContext.Message.Bands.Select(b => b.Slug ?? ""))
            : null;
        album.CountryNames = consumeContext.Message.Countries.Count > 0
            ? string.Join(",", consumeContext.Message.Countries.Select(c => c.Name))
            : null;
        album.CountryCodes = consumeContext.Message.Countries.Count > 0
            ? string.Join(",", consumeContext.Message.Countries.Select(c => c.Code ?? ""))
            : null;

        repo.Update(album);
        await repo.SaveChangesAsync(consumeContext.CancellationToken);
    }
}
