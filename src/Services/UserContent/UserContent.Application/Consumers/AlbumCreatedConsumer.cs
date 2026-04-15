namespace UserContent.Application.Consumers;

public class AlbumCreatedConsumer(IRepository<UserContentContext> repo) : IConsumer<AlbumCreatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<AlbumCreatedIntegrationEvent> consumeContext)
    {
        bool exists = await repo.GetByAsync<Album>(
            a => a.Id == consumeContext.Message.AlbumId,
            cancellationToken: consumeContext.CancellationToken) is not null;

        if (exists) return;

        Album album = new()
        {
            Id = consumeContext.Message.AlbumId,
            Title = consumeContext.Message.Title,
            Slug = consumeContext.Message.Slug,
            CoverUrl = consumeContext.Message.CoverUrl,
            ReleaseDate = consumeContext.Message.ReleaseYear,
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
    }
}
