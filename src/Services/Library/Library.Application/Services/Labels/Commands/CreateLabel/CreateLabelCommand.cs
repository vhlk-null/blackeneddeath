namespace Library.Application.Services.Labels.Commands.CreateLabel;

public record CreateLabelCommand(string Name) : BuildingBlocks.CQRS.ICommand<CreateLabelResult>;

public record CreateLabelResult(Guid Id);

public class CreateLabelCommandValidator : AbstractValidator<CreateLabelCommand>
{
    public CreateLabelCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(ValidationMessages.EmptyRequiredField)
            .MaximumLength(200).WithMessage(ValidationMessages.MaxLengthIsExceeded);
    }
}
