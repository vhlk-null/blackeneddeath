namespace Library.Application.Services.Bands.Commands.UpdateBand;

public record UpdateBandCommand(
    UpdateBandDto Band,
    Stream? Logo = null,
    string? LogoContentType = null,
    string? LogoFileName = null)
    : BuildingBlocks.CQRS.ICommand<UpdateBandResult>;

public record UpdateBandResult(bool IsSuccess);

public class UpdateBandCommandValidator : AbstractValidator<UpdateBandCommand>
{
    public UpdateBandCommandValidator()
    {
        RuleFor(x => x.Band.Name)
            .NotEmpty().WithMessage(ValidationMessages.EmptyRequiredField)
            .MaximumLength(200).WithMessage(ValidationMessages.MaxLengthIsExceeded);

        RuleFor(x => x.Band.FormedYear)
            .InclusiveBetween(1900, 2099).WithMessage(ValidationMessages.BandYearOutOfRange)
            .When(x => x.Band.FormedYear.HasValue);

        RuleFor(x => x.Band.DisbandedYear)
            .InclusiveBetween(1900, 2099).WithMessage(ValidationMessages.BandYearOutOfRange)
            .When(x => x.Band.DisbandedYear.HasValue);

        RuleFor(x => x.Band.DisbandedYear)
            .GreaterThanOrEqualTo(x => x.Band.FormedYear).WithMessage(ValidationMessages.DisbandedYearBeforeFormed)
            .When(x => x.Band.FormedYear.HasValue && x.Band.DisbandedYear.HasValue);
    }
}
