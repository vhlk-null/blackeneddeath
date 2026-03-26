namespace Library.Application.Services.Countries.Commands.DeleteCountry;

public record DeleteCountryCommand(Guid Id) : BuildingBlocks.CQRS.ICommand<DeleteCountryResult>;

public record DeleteCountryResult(bool IsSuccess);

public class DeleteCountryCommandValidator : AbstractValidator<DeleteCountryCommand>
{
    public DeleteCountryCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage(ValidationMessages.EmptyRequiredField);
    }
}
