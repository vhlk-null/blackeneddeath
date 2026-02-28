
namespace Library.Application.Bands.Commands.DeleteBand;

internal class DeleteBandCommandHandler(ILibraryDbContext context)
    : BuildingBlocks.CQRS.ICommandHandler<DeleteBandCommand, DeleteBandResult>
{
    public async ValueTask<DeleteBandResult> Handle(
        DeleteBandCommand command,
        CancellationToken cancellationToken)
    {
        var band = await context.Bands.FindAsync([BandId.Of(command.Id)], cancellationToken)
            ?? throw new BandNotFoundException(command.Id);

        band.Remove();
        context.Bands.Remove(band);
        await context.SaveChangesAsync(cancellationToken);

        return new DeleteBandResult(true);
    }
}
