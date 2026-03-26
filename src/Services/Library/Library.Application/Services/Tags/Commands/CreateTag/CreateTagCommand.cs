namespace Library.Application.Services.Tags.Commands.CreateTag;

public record CreateTagCommand(string Name) : BuildingBlocks.CQRS.ICommand<CreateTagResult>;

public record CreateTagResult(Guid Id);

public class CreateTagCommandValidator : AbstractValidator<CreateTagCommand>
{
    public CreateTagCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(ValidationMessages.EmptyRequiredField)
            .MaximumLength(100).WithMessage(ValidationMessages.MaxLengthIsExceeded);
    }
}
