namespace Library.Application.Bands.Commands.CreateBand;

public class CreateBandCommandHandler(ILibraryDbContext context)
    : BuildingBlocks.CQRS.ICommandHandler<CreateBandCommand, CreateBandResult>
{
    public async ValueTask<CreateBandResult> Handle(CreateBandCommand command, CancellationToken cancellationToken)
    {
        var band = CreateNewBand(command);

        context.Bands.Add(band);
        await context.SaveChangesAsync(cancellationToken);

        return new CreateBandResult(band.Id.Value);
    }

    private static Band CreateNewBand(CreateBandCommand command)
    {
        var activity = BandActivity.Of(command.Band.FormedYear, command.Band.DisbandedYear);

        var band = Band.Create(command.Band.Name, command.Band.Bio, logoUrl: null, activity, command.Band.Status);

        foreach (var country in command.Band.Countries)
            band.AddCountry(CountryId.Of(country.Id));

        foreach (var genre in command.Band.Genres)
            band.AddGenre(GenreId.Of(genre.Id), genre.IsPrimary);

        return band;
    }
}
