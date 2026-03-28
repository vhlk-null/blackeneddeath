namespace Library.Application.Services.Bands.Commands.UpdateBand;

public record UpdateBandCommand(UpdateBandDto Band) : BuildingBlocks.CQRS.ICommand<UpdateBandResult>;

public record UpdateBandResult(bool IsSuccess);

public class UpdateBandCommandValidator : AbstractValidator<UpdateBandCommand>
{
    public UpdateBandCommandValidator()
    {
        RuleFor(x => x.Band.Name)
            .NotEmpty().WithMessage(ValidationMessages.EmptyRequiredField)
            .MaximumLength(200).WithMessage(ValidationMessages.MaxLengthIsExceeded);

        RuleFor(x => x.Band.FormedYear)
            .GreaterThan(1900).When(x => x.Band.FormedYear.HasValue)
            .LessThanOrEqualTo(DateTime.UtcNow.Year).When(x => x.Band.FormedYear.HasValue);

        RuleFor(x => x.Band.DisbandedYear)
            .GreaterThan(1900).When(x => x.Band.DisbandedYear.HasValue)
            .LessThanOrEqualTo(DateTime.UtcNow.Year).When(x => x.Band.DisbandedYear.HasValue)
            .GreaterThanOrEqualTo(x => x.Band.FormedYear).When(x => x.Band.FormedYear.HasValue && x.Band.DisbandedYear.HasValue)
            .WithMessage("Disbanded year must be after formed year");
    }
}
