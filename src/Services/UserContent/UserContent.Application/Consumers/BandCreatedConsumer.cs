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
            Slug = consumeContext.Message.Slug,
            LogoUrl = consumeContext.Message.LogoUrl,
            FormedYear = consumeContext.Message.FormedYear,
            DisbandedYear = consumeContext.Message.DisbandedYear,
            Status = consumeContext.Message.Status,
            PrimaryGenreName = consumeContext.Message.PrimaryGenre?.Name,
            PrimaryGenreSlug = consumeContext.Message.PrimaryGenre?.Slug,
            CountryNames = consumeContext.Message.Countries.Count > 0
                ? string.Join(",", consumeContext.Message.Countries.Select(c => c.Name))
                : null,
            CountryCodes = consumeContext.Message.Countries.Count > 0
                ? string.Join(",", consumeContext.Message.Countries.Select(c => c.Code ?? ""))
                : null
        };

        await repo.AddAsync(band, consumeContext.CancellationToken);
        await repo.SaveChangesAsync(consumeContext.CancellationToken);
    }
}
