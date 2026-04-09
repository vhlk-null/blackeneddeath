namespace Library.Application.Services.Bands.Commands.ApproveBand;

public class ApproveBandCommandHandler(ILibraryDbContext context)
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

        return new ApproveBandResult(true);
    }
}
