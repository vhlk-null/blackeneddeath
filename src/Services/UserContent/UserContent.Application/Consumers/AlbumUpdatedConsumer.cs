namespace UserContent.Application.Consumers;

public class AlbumUpdatedConsumer(IRepository<UserContentContext> repo) : IConsumer<AlbumUpdatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<AlbumUpdatedIntegrationEvent> consumeContext)
    {
        var album = await repo.GetByAsync<Album>(
            a => a.Id == consumeContext.Message.AlbumId,
            cancellationToken: consumeContext.CancellationToken);

        if (album is null) return;

        album.Title = consumeContext.Message.Title;
        album.CoverUrl = consumeContext.Message.CoverUrl;
        album.ReleaseDate = consumeContext.Message.ReleaseDate;

        repo.Update(album);
        await repo.SaveChangesAsync(consumeContext.CancellationToken);
    }
}
