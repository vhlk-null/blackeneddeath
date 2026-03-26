namespace Library.Application.Services.Labels.Commands.DeleteLabel;

public record DeleteLabelCommand(Guid Id) : BuildingBlocks.CQRS.ICommand<DeleteLabelResult>;

public record DeleteLabelResult(bool IsSuccess);

public class DeleteLabelCommandValidator : AbstractValidator<DeleteLabelCommand>
{
    public DeleteLabelCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage(ValidationMessages.EmptyRequiredField);
    }
}
