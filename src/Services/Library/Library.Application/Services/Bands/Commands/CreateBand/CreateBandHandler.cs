using System.Text.RegularExpressions;

namespace Library.Application.Services.Bands.Commands.CreateBand;

public class CreateBandCommandHandler(ILibraryDbContext context, IStorageService storage)
    : BuildingBlocks.CQRS.ICommandHandler<CreateBandCommand, CreateBandResult>
{
    public async ValueTask<CreateBandResult> Handle(CreateBandCommand command, CancellationToken cancellationToken)
    {
        string? logoKey = null;
        if (command.Logo is not null && command.LogoContentType is not null && command.LogoFileName is not null)
        {
            var folder = $"bands/{Slugify(command.Band.Name)}/logo";
            var extension = Path.GetExtension(command.LogoFileName);
            logoKey = await storage.UploadFileAsync(folder, $"{Guid.NewGuid()}{extension}", command.Logo, command.LogoContentType, cancellationToken);
        }

        var band = CreateNewBand(command, logoKey);

        context.Bands.Add(band);
        await context.SaveChangesAsync(cancellationToken);

        return new CreateBandResult(band.Id.Value);
    }

    private static Band CreateNewBand(CreateBandCommand command, string? logoKey)
    {
        var activity = BandActivity.Of(command.Band.FormedYear, command.Band.DisbandedYear);

        var band = Band.Create(command.Band.Name, command.Band.Bio, logoKey, activity, command.Band.Status);

        foreach (var id in command.Band.CountryIds)
            band.AddCountry(CountryId.Of(id));

        if (command.Band.GenreId is Guid primaryGenreId)
            band.AddGenre(GenreId.Of(primaryGenreId), isPrimary: true);

        foreach (var id in command.Band.SubgenreIds ?? [])
            band.AddGenre(GenreId.Of(id), isPrimary: false);

        return band;
    }

    private static string Slugify(string value) =>
        Regex.Replace(value.ToLowerInvariant().Trim(), @"[^a-z0-9]+", "-").Trim('-');
}
