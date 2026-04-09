namespace Library.Application.Services.Bands.Commands.ApproveVideoBand;

public record ApproveVideoBandCommand(Guid VideoBandId) : BuildingBlocks.CQRS.ICommand<ApproveVideoBandResult>;
public record ApproveVideoBandResult(bool IsSuccess);

public class ApproveVideoBandCommandValidator : AbstractValidator<ApproveVideoBandCommand>
{
    public ApproveVideoBandCommandValidator()
    {
        RuleFor(x => x.VideoBandId).NotEmpty().WithMessage(ValidationMessages.EmptyRequiredField);
    }
}
