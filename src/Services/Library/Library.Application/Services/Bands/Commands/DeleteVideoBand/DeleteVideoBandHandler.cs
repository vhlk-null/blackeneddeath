namespace Library.Application.Services.Bands.Commands.DeleteVideoBand;

public class DeleteVideoBandHandler(ILibraryDbContext context)
    : BuildingBlocks.CQRS.ICommandHandler<DeleteVideoBandCommand, DeleteVideoBandResult>
{
    public async ValueTask<DeleteVideoBandResult> Handle(DeleteVideoBandCommand command, CancellationToken cancellationToken)
    {
        var videoBand = await context.VideoBands.FindAsync([VideoBandId.Of(command.Id)], cancellationToken)
            ?? throw new VideoBandNotFoundException(command.Id);

        context.VideoBands.Remove(videoBand);
        await context.SaveChangesAsync(cancellationToken);

        return new DeleteVideoBandResult(true);
    }
}
