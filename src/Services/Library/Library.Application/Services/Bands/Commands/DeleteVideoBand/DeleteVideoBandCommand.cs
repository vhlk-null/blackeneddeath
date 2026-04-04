namespace Library.Application.Services.Bands.Commands.DeleteVideoBand;

public record DeleteVideoBandCommand(Guid Id) : BuildingBlocks.CQRS.ICommand<DeleteVideoBandResult>;

public record DeleteVideoBandResult(bool IsSuccess);

public class DeleteVideoBandCommandValidator : AbstractValidator<DeleteVideoBandCommand>
{
    public DeleteVideoBandCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage(ValidationMessages.EmptyRequiredField);
    }
}
