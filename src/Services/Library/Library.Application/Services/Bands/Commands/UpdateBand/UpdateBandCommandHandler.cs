namespace Library.Application.Services.Bands.Commands.UpdateBand;

public class UpdateBandCommandHandler(ILibraryDbContext context) : BuildingBlocks.CQRS.ICommandHandler<UpdateBandCommand, UpdateBandResult>
{
    public async ValueTask<UpdateBandResult> Handle(UpdateBandCommand command, CancellationToken cancellationToken)
    {
        var band = await context.Bands
            .Include(b => b.BandCountries)
            .Include(b => b.BandGenres)
            .FirstOrDefaultAsync(b => b.Id == BandId.Of(command.Band.Id), cancellationToken)
            ?? throw new BandNotFoundException(command.Band.Id);

        UpdateBand(band, command);

        await context.SaveChangesAsync(cancellationToken);

        return new UpdateBandResult(true);
    }

    private static void UpdateBand(Band band, UpdateBandCommand command)
    {
        var activity = BandActivity.Of(command.Band.FormedYear, command.Band.DisbandedYear);

        band.Update(command.Band.Name, command.Band.Bio, band.LogoUrl, activity, command.Band.Status);

        ReconcileCountries(band, command.Band.CountryIds);
        ReconcileGenres(band, command.Band.GenreId, command.Band.SubgenreIds);
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
}
