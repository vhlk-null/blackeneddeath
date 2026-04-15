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
                CoverUrl = consumeContext.Message.CoverUrl,
                ReleaseDate = consumeContext.Message.ReleaseDate
            };
            await repo.AddAsync(album, consumeContext.CancellationToken);
            await repo.SaveChangesAsync(consumeContext.CancellationToken);
            return;
        }

        album.Title = consumeContext.Message.Title;
        album.CoverUrl = consumeContext.Message.CoverUrl;
        album.ReleaseDate = consumeContext.Message.ReleaseDate;

        repo.Update(album);
        await repo.SaveChangesAsync(consumeContext.CancellationToken);
    }
}
