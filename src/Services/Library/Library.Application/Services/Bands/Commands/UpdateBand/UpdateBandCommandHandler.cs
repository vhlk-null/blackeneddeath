using System.Text.RegularExpressions;

namespace Library.Application.Services.Bands.Commands.UpdateBand;

public class UpdateBandCommandHandler(ILibraryDbContext context, IStorageService storage) : BuildingBlocks.CQRS.ICommandHandler<UpdateBandCommand, UpdateBandResult>
{
    public async ValueTask<UpdateBandResult> Handle(UpdateBandCommand command, CancellationToken cancellationToken)
    {
        Band band = await context.Bands
                        .Include(b => b.BandCountries)
                        .Include(b => b.BandGenres)
                        .FirstOrDefaultAsync(b => b.Id == BandId.Of(command.Band.Id), cancellationToken)
                    ?? throw new BandNotFoundException(command.Band.Id);

        string? logoKey = band.LogoUrl;
        if (command.Logo is not null && command.LogoContentType is not null && command.LogoFileName is not null)
        {
            if (band.LogoUrl is not null)
                await storage.DeleteFileAsync(band.LogoUrl, cancellationToken);

            string folder = $"bands/{Slugify(command.Band.Name)}/logo";
            string extension = Path.GetExtension(command.LogoFileName);
            logoKey = await storage.UploadFileAsync(folder, $"{Guid.NewGuid()}{extension}", command.Logo, command.LogoContentType, cancellationToken);
        }

        BandActivity activity = BandActivity.Of(command.Band.FormedYear, command.Band.DisbandedYear);
        band.Update(command.Band.Name, command.Band.Bio, logoKey, activity, command.Band.Status,
            command.Band.Facebook, command.Band.Youtube, command.Band.Instagram, command.Band.Twitter, command.Band.Website);
        band.Approve();

        ReconcileCountries(band, command.Band.CountryIds);
        ReconcileGenres(band, command.Band.GenreIds);

        await context.SaveChangesAsync(cancellationToken);

        return new UpdateBandResult(true);
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

    private static string Slugify(string value) =>
        Regex.Replace(value.ToLowerInvariant().Trim(), @"[^a-z0-9]+", "-").Trim('-');
}
