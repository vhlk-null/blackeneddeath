namespace Library.Application.Services.Bands.Commands.ApproveBand;

public record ApproveBandCommand(Guid BandId) : BuildingBlocks.CQRS.ICommand<ApproveBandResult>;
public record ApproveBandResult(bool IsSuccess);

public class ApproveBandCommandValidator : AbstractValidator<ApproveBandCommand>
{
    public ApproveBandCommandValidator()
    {
        RuleFor(x => x.BandId).NotEmpty().WithMessage(ValidationMessages.EmptyRequiredField);
    }
}
