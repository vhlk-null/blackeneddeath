namespace Library.Application.Services.Bands.Commands.ApproveBand;

public class ApproveBandCommandHandler(ILibraryDbContext context, IAlbumDetailCache albumDetailCache)
    : BuildingBlocks.CQRS.ICommandHandler<ApproveBandCommand, ApproveBandResult>
{
    public async ValueTask<ApproveBandResult> Handle(
        ApproveBandCommand command,
        CancellationToken cancellationToken)
    {
        Band band = await context.Bands.FindAsync([BandId.Of(command.BandId)], cancellationToken)
                    ?? throw new BandNotFoundException(command.BandId);

        band.Approve();
        await context.SaveChangesAsync(cancellationToken);

        await albumDetailCache.InvalidateForBandsAsync([command.BandId], cancellationToken);

        return new ApproveBandResult(true);
    }
}
