namespace Library.Application.Services.Tags.Commands.DeleteTag;

public class DeleteTagCommandHandler(ILibraryDbContext context)
    : BuildingBlocks.CQRS.ICommandHandler<DeleteTagCommand, DeleteTagResult>
{
    public async ValueTask<DeleteTagResult> Handle(DeleteTagCommand command, CancellationToken cancellationToken)
    {
        Tag tag = await context.Tags.FindAsync([TagId.Of(command.Id)], cancellationToken)
                  ?? throw new TagNotFoundException(command.Id);

        context.Tags.Remove(tag);
        await context.SaveChangesAsync(cancellationToken);

        return new DeleteTagResult(true);
    }
}
