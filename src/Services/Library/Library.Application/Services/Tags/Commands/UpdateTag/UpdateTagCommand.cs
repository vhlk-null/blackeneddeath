namespace Library.Application.Services.Tags.Commands.UpdateTag;

public record UpdateTagCommand(Guid Id, string Name) : BuildingBlocks.CQRS.ICommand<UpdateTagResult>;

public record UpdateTagResult(bool IsSuccess);

public class UpdateTagCommandValidator : AbstractValidator<UpdateTagCommand>
{
    public UpdateTagCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage(ValidationMessages.EmptyRequiredField);

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(ValidationMessages.EmptyRequiredField)
            .MaximumLength(100).WithMessage(ValidationMessages.MaxLengthIsExceeded);
    }
}
