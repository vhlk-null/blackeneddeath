namespace Library.Application.Services.Countries.Commands.CreateCountry;

public record CreateCountryCommand(string Name, string Code) : BuildingBlocks.CQRS.ICommand<CreateCountryResult>;

public record CreateCountryResult(Guid Id);

public class CreateCountryCommandValidator : AbstractValidator<CreateCountryCommand>
{
    public CreateCountryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(ValidationMessages.EmptyRequiredField)
            .MaximumLength(200).WithMessage(ValidationMessages.MaxLengthIsExceeded);

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage(ValidationMessages.EmptyRequiredField)
            .Length(2).WithMessage(ValidationMessages.InvalidFieldValue);
    }
}
