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
            CoverUrl = consumeContext.Message.CoverUrl,
            ReleaseDate = consumeContext.Message.ReleaseYear
        };

        await repo.AddAsync(album, consumeContext.CancellationToken);
        await repo.SaveChangesAsync(consumeContext.CancellationToken);
    }
}
