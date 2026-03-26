namespace Library.Application.Services.Labels.Commands.CreateLabel;

public class CreateLabelCommandHandler(ILibraryDbContext context)
    : BuildingBlocks.CQRS.ICommandHandler<CreateLabelCommand, CreateLabelResult>
{
    public async ValueTask<CreateLabelResult> Handle(CreateLabelCommand command, CancellationToken cancellationToken)
    {
        var label = Label.Create(LabelId.Of(Guid.NewGuid()), command.Name);

        context.Labels.Add(label);
        await context.SaveChangesAsync(cancellationToken);

        return new CreateLabelResult(label.Id.Value);
    }
}
