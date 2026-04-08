namespace Library.Application.Services.Labels.Commands.DeleteLabel;

public class DeleteLabelCommandHandler(ILibraryDbContext context)
    : BuildingBlocks.CQRS.ICommandHandler<DeleteLabelCommand, DeleteLabelResult>
{
    public async ValueTask<DeleteLabelResult> Handle(DeleteLabelCommand command, CancellationToken cancellationToken)
    {
        Label label = await context.Labels.FindAsync([LabelId.Of(command.Id)], cancellationToken)
                      ?? throw new LabelNotFoundException(command.Id);

        context.Labels.Remove(label);
        await context.SaveChangesAsync(cancellationToken);

        return new DeleteLabelResult(true);
    }
}
