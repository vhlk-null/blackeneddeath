using System.Text.RegularExpressions;

namespace Library.Application.Services.Bands.Commands.CreateBand;

public class CreateBandCommandHandler(ILibraryDbContext context, IStorageService storage, IHttpContextAccessor httpContextAccessor)
    : BuildingBlocks.CQRS.ICommandHandler<CreateBandCommand, CreateBandResult>
{
    public async ValueTask<CreateBandResult> Handle(CreateBandCommand command, CancellationToken cancellationToken)
    {
        await ValidateReferencedEntitiesAsync(command.Band, cancellationToken);

        string? logoKey = null;
        if (command.Logo is not null && command.LogoContentType is not null && command.LogoFileName is not null)
        {
            string folder = $"bands/{Slugify(command.Band.Name)}/logo";
            string extension = Path.GetExtension(command.LogoFileName);
            logoKey = await storage.UploadFileAsync(folder, $"{Guid.NewGuid()}{extension}", command.Logo, command.LogoContentType, cancellationToken);
        }

        Band band = CreateNewBand(command, logoKey);

        if (httpContextAccessor.HttpContext?.User.IsInRole("admin") == true)
            band.Approve();

        context.Bands.Add(band);
        await context.SaveChangesAsync(cancellationToken);

        return new CreateBandResult(band.Id.Value);
    }

    private async Task ValidateReferencedEntitiesAsync(CreateBandDto band, CancellationToken cancellationToken)
    {
        foreach (Guid id in band.CountryIds)
        {
            if (!await context.Countries.AnyAsync(c => c.Id == CountryId.Of(id), cancellationToken))
                throw new CountryNotFoundException(id);
        }

        foreach (Guid id in band.GenreIds)
        {
            if (!await context.Genres.AnyAsync(g => g.Id == GenreId.Of(id), cancellationToken))
                throw new GenreNotFoundException(id);
        }
    }

    private static Band CreateNewBand(CreateBandCommand command, string? logoKey)
    {
        BandActivity activity = BandActivity.Of(command.Band.FormedYear, command.Band.DisbandedYear);

        Band band = Band.Create(command.Band.Name, command.Band.Bio, logoKey, activity, command.Band.Status,
            facebook: command.Band.Facebook, youtube: command.Band.Youtube, instagram: command.Band.Instagram,
            twitter: command.Band.Twitter, website: command.Band.Website);

        foreach (Guid id in command.Band.CountryIds)
            band.AddCountry(CountryId.Of(id));

        foreach ((Guid id, int index) in command.Band.GenreIds.Select((id, i) => (id, i)))
            band.AddGenre(GenreId.Of(id), isPrimary: index == 0);

        return band;
    }

    private static string Slugify(string value) =>
        Regex.Replace(value.ToLowerInvariant().Trim(), @"[^a-z0-9]+", "-").Trim('-');
}
