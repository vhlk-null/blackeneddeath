using System.Text.RegularExpressions;

namespace Library.Application.Services.Bands.Commands.UpdateBand;

public class UpdateBandCommandHandler(ILibraryDbContext context, IStorageService storage) : BuildingBlocks.CQRS.ICommandHandler<UpdateBandCommand, UpdateBandResult>
{
    public async ValueTask<UpdateBandResult> Handle(UpdateBandCommand command, CancellationToken cancellationToken)
    {
        var band = await context.Bands
            .Include(b => b.BandCountries)
            .Include(b => b.BandGenres)
            .FirstOrDefaultAsync(b => b.Id == BandId.Of(command.Band.Id), cancellationToken)
            ?? throw new BandNotFoundException(command.Band.Id);

        var logoKey = band.LogoUrl;
        if (command.Logo is not null && command.LogoContentType is not null && command.LogoFileName is not null)
        {
            if (band.LogoUrl is not null)
                await storage.DeleteFileAsync(band.LogoUrl, cancellationToken);

            var folder = $"bands/{Slugify(command.Band.Name)}/logo";
            var extension = Path.GetExtension(command.LogoFileName);
            logoKey = await storage.UploadFileAsync(folder, $"{Guid.NewGuid()}{extension}", command.Logo, command.LogoContentType, cancellationToken);
        }

        var activity = BandActivity.Of(command.Band.FormedYear, command.Band.DisbandedYear);
        band.Update(command.Band.Name, command.Band.Bio, logoKey, activity, command.Band.Status,
            command.Band.Facebook, command.Band.Youtube, command.Band.Instagram, command.Band.Twitter, command.Band.Website);

        ReconcileCountries(band, command.Band.CountryIds);
        ReconcileGenres(band, command.Band.GenreId, command.Band.SubgenreIds);

        await context.SaveChangesAsync(cancellationToken);

        return new UpdateBandResult(true);
    }

    private static void ReconcileCountries(Band band, List<Guid> incomingIds)
    {
        var currentIds = band.BandCountries.Select(x => x.CountryId).ToHashSet();
        var incoming = incomingIds.Select(CountryId.Of).ToHashSet();

        foreach (var id in currentIds.Except(incoming))
            band.RemoveCountry(id);

        foreach (var id in incoming.Except(currentIds))
            band.AddCountry(id);
    }

    private static void ReconcileGenres(Band band, Guid? genreId, List<Guid>? subgenreIds)
    {
        var currentIds = band.BandGenres.Select(x => x.GenreId).ToHashSet();

        var incomingPrimary = genreId is Guid pid ? GenreId.Of(pid) : null;
        var incomingSubs = (subgenreIds ?? []).Select(GenreId.Of).ToHashSet();
        var allIncoming = incomingSubs.ToHashSet();
        if (incomingPrimary != null) allIncoming.Add(incomingPrimary);

        foreach (var id in currentIds.Except(allIncoming))
            band.RemoveGenre(id);

        if (incomingPrimary != null && !currentIds.Contains(incomingPrimary))
            band.AddGenre(incomingPrimary, isPrimary: true);

        foreach (var id in incomingSubs.Where(id => !currentIds.Contains(id)))
            band.AddGenre(id, isPrimary: false);
    }

    private static string Slugify(string value) =>
        Regex.Replace(value.ToLowerInvariant().Trim(), @"[^a-z0-9]+", "-").Trim('-');
}
