namespace Library.Application.Bands.Commands.DeleteBand;

public record DeleteBandCommand(Guid Id) : ICommand<DeleteBandResult>;

public record DeleteBandResult(bool IsSuccess);

public class DeleteBandCommandValidator : AbstractValidator<DeleteBandCommand>
{
    public DeleteBandCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage(ValidationMessages.EmptyRequiredField);
    }
}
