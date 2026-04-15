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
            return;
        }

        band.BandName = consumeContext.Message.Name;
        band.Slug = consumeContext.Message.Slug;
        band.LogoUrl = consumeContext.Message.LogoUrl;
        band.FormedYear = consumeContext.Message.FormedYear;
        band.DisbandedYear = consumeContext.Message.DisbandedYear;
        band.Status = consumeContext.Message.Status;
        band.PrimaryGenreName = consumeContext.Message.PrimaryGenre?.Name;
        band.PrimaryGenreSlug = consumeContext.Message.PrimaryGenre?.Slug;
        band.CountryNames = consumeContext.Message.Countries.Count > 0
            ? string.Join(",", consumeContext.Message.Countries.Select(c => c.Name))
            : null;
        band.CountryCodes = consumeContext.Message.Countries.Count > 0
            ? string.Join(",", consumeContext.Message.Countries.Select(c => c.Code ?? ""))
            : null;

        repo.Update(band);
        await repo.SaveChangesAsync(consumeContext.CancellationToken);
    }
}
