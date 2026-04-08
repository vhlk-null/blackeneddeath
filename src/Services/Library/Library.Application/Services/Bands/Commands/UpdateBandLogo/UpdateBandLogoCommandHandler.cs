using System.Text.RegularExpressions;

namespace Library.Application.Services.Bands.Commands.UpdateBandLogo;

public class UpdateBandLogoCommandHandler(ILibraryDbContext context, IStorageService storage)
    : BuildingBlocks.CQRS.ICommandHandler<UpdateBandLogoCommand, UpdateBandLogoResult>
{
    public async ValueTask<UpdateBandLogoResult> Handle(UpdateBandLogoCommand command, CancellationToken cancellationToken)
    {
        Band band = await context.Bands.FindAsync([BandId.Of(command.BandId)], cancellationToken)
                    ?? throw new BandNotFoundException(command.BandId);

        if (band.LogoUrl is not null)
            await storage.DeleteFileAsync(band.LogoUrl, cancellationToken);

        string folder = $"bands/{Slugify(band.Name)}/logo";
        string extension = Path.GetExtension(command.LogoFileName);
        string logoKey = await storage.UploadFileAsync(folder, $"{Guid.NewGuid()}{extension}", command.Logo, command.LogoContentType, cancellationToken);

        band.Update(band.Name, band.Bio, logoKey, band.Activity, band.Status);
        await context.SaveChangesAsync(cancellationToken);

        return new UpdateBandLogoResult(true);
    }

    private static string Slugify(string value) =>
        Regex.Replace(value.ToLowerInvariant().Trim(), @"[^a-z0-9]+", "-").Trim('-');
}
