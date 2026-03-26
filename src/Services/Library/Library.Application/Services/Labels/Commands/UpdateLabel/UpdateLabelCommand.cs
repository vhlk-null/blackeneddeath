namespace Library.Application.Services.Labels.Commands.UpdateLabel;

public record UpdateLabelCommand(Guid Id, string Name) : BuildingBlocks.CQRS.ICommand<UpdateLabelResult>;

public record UpdateLabelResult(bool IsSuccess);

public class UpdateLabelCommandValidator : AbstractValidator<UpdateLabelCommand>
{
    public UpdateLabelCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage(ValidationMessages.EmptyRequiredField);

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(ValidationMessages.EmptyRequiredField)
            .MaximumLength(200).WithMessage(ValidationMessages.MaxLengthIsExceeded);
    }
}
