namespace Library.Application.Services.Bands.Commands.CreateBand;

public record CreateBandCommand(CreateBandDto Band, Stream? Logo = null, string? LogoContentType = null, string? LogoFileName = null) : BuildingBlocks.CQRS.ICommand<CreateBandResult>;

public record CreateBandResult(Guid Id);

public class CreateBandCommandValidator : AbstractValidator<CreateBandCommand>
{
    public CreateBandCommandValidator()
    {
        RuleFor(x => x.Band.Name)
            .NotEmpty().WithMessage(ValidationMessages.EmptyRequiredField)
            .MaximumLength(200).WithMessage(ValidationMessages.MaxLengthIsExceeded);

        RuleFor(x => x.Band.CountryIds)
            .NotEmpty().WithMessage(ValidationMessages.EmptyRequiredField);

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
