namespace UserContent.Application.Consumers;

public class BandRemovedConsumer(IRepository<UserContentContext> repo) : IConsumer<BandRemovedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<BandRemovedIntegrationEvent> consumeContext)
    {
        Band? band = await repo.GetByAsync<Band>(
            b => b.BandId == consumeContext.Message.BandId,
            cancellationToken: consumeContext.CancellationToken);

        if (band is null) return;

        repo.Delete(band);
        await repo.SaveChangesAsync(consumeContext.CancellationToken);
    }
}
