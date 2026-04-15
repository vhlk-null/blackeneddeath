namespace UserContent.Application.Consumers;

public class BandCreatedConsumer(IRepository<UserContentContext> repo) : IConsumer<BandCreatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<BandCreatedIntegrationEvent> consumeContext)
    {
        bool exists = await repo.GetByAsync<Band>(
            b => b.BandId == consumeContext.Message.BandId,
            cancellationToken: consumeContext.CancellationToken) is not null;

        if (exists) return;

        Band band = new()
        {
            BandId = consumeContext.Message.BandId,
            BandName = consumeContext.Message.Name,
            LogoUrl = consumeContext.Message.LogoUrl
        };

        await repo.AddAsync(band, consumeContext.CancellationToken);
        await repo.SaveChangesAsync(consumeContext.CancellationToken);
    }
}
