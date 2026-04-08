namespace Library.Application.Services.Tags.Commands.UpdateTag;

public class UpdateTagCommandHandler(ILibraryDbContext context)
    : BuildingBlocks.CQRS.ICommandHandler<UpdateTagCommand, UpdateTagResult>
{
    public async ValueTask<UpdateTagResult> Handle(UpdateTagCommand command, CancellationToken cancellationToken)
    {
        Tag tag = await context.Tags.FindAsync([TagId.Of(command.Id)], cancellationToken)
                  ?? throw new TagNotFoundException(command.Id);

        tag.Update(command.Name);
        await context.SaveChangesAsync(cancellationToken);

        return new UpdateTagResult(true);
    }
}
