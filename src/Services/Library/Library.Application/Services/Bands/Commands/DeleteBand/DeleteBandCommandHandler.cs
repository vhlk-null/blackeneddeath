
namespace Library.Application.Services.Bands.Commands.DeleteBand;

public class DeleteBandCommandHandler(ILibraryDbContext context, IStorageService storage)
    : BuildingBlocks.CQRS.ICommandHandler<DeleteBandCommand, DeleteBandResult>
{
    public async ValueTask<DeleteBandResult> Handle(
        DeleteBandCommand command,
        CancellationToken cancellationToken)
    {
        Band band = await context.Bands.FindAsync([BandId.Of(command.Id)], cancellationToken)
                    ?? throw new BandNotFoundException(command.Id);

        if (band.LogoUrl is not null)
            await storage.DeleteFileAsync(band.LogoUrl, cancellationToken);

        band.Remove();
        context.Bands.Remove(band);
        await context.SaveChangesAsync(cancellationToken);

        return new DeleteBandResult(true);
    }
}
