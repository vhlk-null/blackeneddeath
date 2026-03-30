namespace Library.Application.Services.GenreCards.Commands.CreateGenreCard;

public record CreateGenreCardCommand(
    string Name,
    string Description,
    Stream? CoverImage = null,
    string? CoverImageContentType = null,
    string? CoverImageFileName = null)
    : BuildingBlocks.CQRS.ICommand<CreateGenreCardResult>;

public record CreateGenreCardResult(Guid Id);

public class CreateGenreCardCommandValidator : AbstractValidator<CreateGenreCardCommand>
{
    public CreateGenreCardCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(ValidationMessages.EmptyRequiredField)
            .MaximumLength(200).WithMessage(ValidationMessages.MaxLengthIsExceeded);

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage(ValidationMessages.EmptyRequiredField)
            .MaximumLength(1000).WithMessage(ValidationMessages.MaxLengthIsExceeded);
    }
}
