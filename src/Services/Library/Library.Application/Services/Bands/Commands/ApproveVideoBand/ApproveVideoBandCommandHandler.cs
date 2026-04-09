namespace Library.Application.Services.Bands.Commands.ApproveVideoBand;

public class ApproveVideoBandCommandHandler(ILibraryDbContext context)
    : BuildingBlocks.CQRS.ICommandHandler<ApproveVideoBandCommand, ApproveVideoBandResult>
{
    public async ValueTask<ApproveVideoBandResult> Handle(
        ApproveVideoBandCommand command,
        CancellationToken cancellationToken)
    {
        VideoBand videoBand = await context.VideoBands.FindAsync([VideoBandId.Of(command.VideoBandId)], cancellationToken)
                              ?? throw new VideoBandNotFoundException(command.VideoBandId);

        videoBand.Approve();
        await context.SaveChangesAsync(cancellationToken);

        return new ApproveVideoBandResult(true);
    }
}
