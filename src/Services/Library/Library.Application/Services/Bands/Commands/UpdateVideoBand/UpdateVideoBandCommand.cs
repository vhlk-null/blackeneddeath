namespace Library.Application.Services.Bands.Commands.UpdateVideoBand;

public record UpdateVideoBandCommand(UpdateVideoBandDto VideoBand) : BuildingBlocks.CQRS.ICommand<UpdateVideoBandResult>;

public record UpdateVideoBandResult(bool IsSuccess);

public class UpdateVideoBandCommandValidator : AbstractValidator<UpdateVideoBandCommand>
{
    public UpdateVideoBandCommandValidator()
    {
        RuleFor(x => x.VideoBand.Id).NotEmpty().WithMessage(ValidationMessages.EmptyRequiredField);
        RuleFor(x => x.VideoBand.BandId).NotEmpty().WithMessage(ValidationMessages.EmptyRequiredField);
        RuleFor(x => x.VideoBand.Name).NotEmpty().WithMessage(ValidationMessages.EmptyRequiredField)
            .MaximumLength(300).WithMessage(ValidationMessages.MaxLengthIsExceeded);
        RuleFor(x => x.VideoBand.YoutubeLink).NotEmpty().WithMessage(ValidationMessages.EmptyRequiredField)
            .MaximumLength(500).WithMessage(ValidationMessages.MaxLengthIsExceeded);
        RuleFor(x => x.VideoBand.VideoType).IsInEnum();
        RuleFor(x => x.VideoBand.Year)
            .GreaterThan(1900).When(x => x.VideoBand.Year.HasValue)
            .LessThanOrEqualTo(DateTime.UtcNow.Year).When(x => x.VideoBand.Year.HasValue);
    }
}
