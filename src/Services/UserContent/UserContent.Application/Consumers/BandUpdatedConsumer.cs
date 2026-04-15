namespace UserContent.Application.Consumers;

public class BandUpdatedConsumer(IRepository<UserContentContext> repo) : IConsumer<BandUpdatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<BandUpdatedIntegrationEvent> consumeContext)
    {
        Band? band = await repo.GetByAsync<Band>(
            b => b.BandId == consumeContext.Message.BandId,
            cancellationToken: consumeContext.CancellationToken);

        if (band is null)
        {
            band = new Band
            {
                BandId = consumeContext.Message.BandId,
                BandName = consumeContext.Message.Name,
                LogoUrl = consumeContext.Message.LogoUrl
            };
            await repo.AddAsync(band, consumeContext.CancellationToken);
            await repo.SaveChangesAsync(consumeContext.CancellationToken);
            return;
        }

        band.BandName = consumeContext.Message.Name;
        band.LogoUrl = consumeContext.Message.LogoUrl;

        repo.Update(band);
        await repo.SaveChangesAsync(consumeContext.CancellationToken);
    }
}
