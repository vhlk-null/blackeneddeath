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

        RuleFor(x => x.Band.FormedYear)
            .GreaterThan(0).When(x => x.Band.FormedYear.HasValue)
            .GreaterThan(1900).When(x => x.Band.FormedYear.HasValue)
            .LessThanOrEqualTo(DateTime.UtcNow.Year).When(x => x.Band.FormedYear.HasValue);

        RuleFor(x => x.Band.DisbandedYear)
            .GreaterThan(0).When(x => x.Band.DisbandedYear.HasValue)
            .GreaterThan(1900).When(x => x.Band.DisbandedYear.HasValue)
            .LessThanOrEqualTo(DateTime.UtcNow.Year).When(x => x.Band.DisbandedYear.HasValue)
            .GreaterThanOrEqualTo(x => x.Band.FormedYear).When(x => x.Band.FormedYear.HasValue && x.Band.DisbandedYear.HasValue)
            .WithMessage("Disbanded year must be after formed year");
    }
}
