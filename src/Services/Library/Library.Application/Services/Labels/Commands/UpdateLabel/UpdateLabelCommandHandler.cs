namespace Library.Application.Services.Labels.Commands.UpdateLabel;

public class UpdateLabelCommandHandler(ILibraryDbContext context)
    : BuildingBlocks.CQRS.ICommandHandler<UpdateLabelCommand, UpdateLabelResult>
{
    public async ValueTask<UpdateLabelResult> Handle(UpdateLabelCommand command, CancellationToken cancellationToken)
    {
        var label = await context.Labels.FindAsync([LabelId.Of(command.Id)], cancellationToken)
            ?? throw new LabelNotFoundException(command.Id);

        label.Update(command.Name);
        await context.SaveChangesAsync(cancellationToken);

        return new UpdateLabelResult(true);
    }
}
