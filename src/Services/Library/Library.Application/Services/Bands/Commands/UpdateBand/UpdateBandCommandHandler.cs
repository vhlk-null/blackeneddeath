namespace Library.Application.Services.Bands.Commands.UpdateBand;

public class UpdateBandCommandHandler(ILibraryDbContext context) : BuildingBlocks.CQRS.ICommandHandler<UpdateBandCommand, UpdateBandResult>
{
    public async ValueTask<UpdateBandResult> Handle(UpdateBandCommand command, CancellationToken cancellationToken)
    {
        var band = await context.Bands
            .Include(b => b.BandCountries)
            .Include(b => b.BandGenres)
            .FirstOrDefaultAsync(b => b.Id.Value == command.Band.Id, cancellationToken)
            ?? throw new BandNotFoundException(command.Band.Id);

        UpdateBand(band, command);

        await context.SaveChangesAsync(cancellationToken);

        return new UpdateBandResult(true);
    }

    private static void UpdateBand(Band band, UpdateBandCommand command)
    {
        var activity = BandActivity.Of(command.Band.FormedYear, command.Band.DisbandedYear);

        band.Update(command.Band.Name, command.Band.Bio, band.LogoUrl, activity, command.Band.Status);

        ReconcileCountries(band, command.Band.Countries);
        ReconcileGenres(band, command.Band.Genres);
    }

    private static void ReconcileCountries(Band band, List<CountryDto> incomingCountries)
    {
        var currentIds = band.BandCountries.Select(x => x.CountryId).ToHashSet();
        var incomingIds = incomingCountries.Select(x => CountryId.Of(x.Id)).ToHashSet();

        foreach (var id in currentIds.Except(incomingIds))
            band.RemoveCountry(id);

        foreach (var id in incomingIds.Except(currentIds))
            band.AddCountry(id);
    }

    private static void ReconcileGenres(Band band, List<GenreDto> incomingGenres)
    {
        var currentIds = band.BandGenres.Select(x => x.GenreId).ToHashSet();
        var incomingIds = incomingGenres.Select(x => GenreId.Of(x.Id)).ToHashSet();

        foreach (var id in currentIds.Except(incomingIds))
            band.RemoveGenre(id);

        foreach (var genre in incomingGenres.Where(g => !currentIds.Contains(GenreId.Of(g.Id))))
            band.AddGenre(GenreId.Of(genre.Id), genre.IsPrimary);
    }
}
