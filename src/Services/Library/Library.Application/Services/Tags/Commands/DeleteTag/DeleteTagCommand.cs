namespace Library.Application.Services.Tags.Commands.DeleteTag;

public record DeleteTagCommand(Guid Id) : BuildingBlocks.CQRS.ICommand<DeleteTagResult>;

public record DeleteTagResult(bool IsSuccess);

public class DeleteTagCommandValidator : AbstractValidator<DeleteTagCommand>
{
    public DeleteTagCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage(ValidationMessages.EmptyRequiredField);
    }
}
