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

        Band? existing = await context.Bands
            .Include(b => b.BandCountries)
            .Include(b => b.BandGenres)
            .FirstOrDefaultAsync(b => b.Name.ToLower() == command.Band.Name.ToLower().Trim(), cancellationToken);

        Band band;
        if (existing is not null)
        {
            BandActivity activity = BandActivity.Of(command.Band.FormedYear, command.Band.DisbandedYear);
            existing.Update(command.Band.Name, command.Band.Bio, logoKey ?? existing.LogoUrl, activity, command.Band.Status,
                command.Band.Facebook, command.Band.Youtube, command.Band.Instagram, command.Band.Twitter, command.Band.Website);

            ReconcileCountries(existing, command.Band.CountryIds);
            ReconcileGenres(existing, command.Band.GenreIds);

            if (httpContextAccessor.HttpContext?.User.IsInRole("admin") == true)
                existing.Approve();

            band = existing;
        }
        else
        {
            band = CreateNewBand(command, logoKey);

            if (httpContextAccessor.HttpContext?.User.IsInRole("admin") == true)
                band.Approve();

            context.Bands.Add(band);
        }

        await context.SaveChangesAsync(cancellationToken);

        return new CreateBandResult(band.Id.Value);
    }

    private static void ReconcileCountries(Band band, List<Guid> incomingIds)
    {
        HashSet<CountryId> currentIds = band.BandCountries.Select(x => x.CountryId).ToHashSet();
        HashSet<CountryId> incoming = incomingIds.Select(CountryId.Of).ToHashSet();

        foreach (CountryId id in currentIds.Except(incoming))
            band.RemoveCountry(id);

        foreach (CountryId id in incoming.Except(currentIds))
            band.AddCountry(id);
    }

    private static void ReconcileGenres(Band band, List<Guid> genreIds)
    {
        HashSet<GenreId> currentIds = band.BandGenres.Select(x => x.GenreId).ToHashSet();
        List<GenreId> incomingOrdered = genreIds.Select(GenreId.Of).ToList();
        HashSet<GenreId> incomingIds = incomingOrdered.ToHashSet();

        foreach (GenreId id in currentIds.Except(incomingIds))
            band.RemoveGenre(id);

        foreach ((GenreId genreId, int index) in incomingOrdered.Select((id, i) => (id, i)))
        {
            if (currentIds.Contains(genreId))
                band.UpdateGenrePrimary(genreId, isPrimary: index == 0);
            else
                band.AddGenre(genreId, isPrimary: index == 0);
        }
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
