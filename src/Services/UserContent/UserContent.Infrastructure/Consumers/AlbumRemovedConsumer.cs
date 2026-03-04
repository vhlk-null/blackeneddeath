namespace UserContent.Infrastructure.Consumers;

public class AlbumRemovedConsumer(IRepository<UserContentContext> repo) : IConsumer<AlbumRemovedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<AlbumRemovedIntegrationEvent> consumeContext)
    {
        var album = await repo.GetByAsync<Album>(
            a => a.Id == consumeContext.Message.AlbumId,
            cancellationToken: consumeContext.CancellationToken);

        if (album is null) return;

        repo.Delete(album);
        await repo.SaveChangesAsync(consumeContext.CancellationToken);
    }
}
