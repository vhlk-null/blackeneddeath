namespace Library.Application.Services.Tags.Commands.CreateTag;

public class CreateTagCommandHandler(ILibraryDbContext context)
    : BuildingBlocks.CQRS.ICommandHandler<CreateTagCommand, CreateTagResult>
{
    public async ValueTask<CreateTagResult> Handle(CreateTagCommand command, CancellationToken cancellationToken)
    {
        var tag = Tag.Create(TagId.Of(Guid.NewGuid()), command.Name);

        context.Tags.Add(tag);
        await context.SaveChangesAsync(cancellationToken);

        return new CreateTagResult(tag.Id.Value);
    }
}
