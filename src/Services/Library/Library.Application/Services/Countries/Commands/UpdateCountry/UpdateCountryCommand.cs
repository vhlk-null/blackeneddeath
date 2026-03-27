namespace Library.Application.Services.Countries.Commands.UpdateCountry;

public record UpdateCountryCommand(Guid Id, string Name, string? Code) : BuildingBlocks.CQRS.ICommand<UpdateCountryResult>;

public record UpdateCountryResult(bool IsSuccess);

public class UpdateCountryCommandValidator : AbstractValidator<UpdateCountryCommand>
{
    public UpdateCountryCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage(ValidationMessages.EmptyRequiredField);

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(ValidationMessages.EmptyRequiredField)
            .MaximumLength(200).WithMessage(ValidationMessages.MaxLengthIsExceeded);

        RuleFor(x => x.Code)
            .Length(2).WithMessage(ValidationMessages.InvalidFieldValue)
            .When(x => x.Code is not null);
    }
}
