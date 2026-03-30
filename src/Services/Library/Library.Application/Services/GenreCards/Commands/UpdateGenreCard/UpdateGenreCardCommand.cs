namespace Library.Application.Services.GenreCards.Commands.UpdateGenreCard;

public record UpdateGenreCardCommand(
    Guid Id,
    string Name,
    string Description,
    int OrderNumber,
    List<Guid> GenreIds,
    List<Guid> TagIds,
    Stream? CoverImage = null,
    string? CoverImageContentType = null,
    string? CoverImageFileName = null)
    : BuildingBlocks.CQRS.ICommand<UpdateGenreCardResult>;

public record UpdateGenreCardResult(bool IsSuccess);

public class UpdateGenreCardCommandValidator : AbstractValidator<UpdateGenreCardCommand>
{
    public UpdateGenreCardCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(ValidationMessages.EmptyRequiredField)
            .MaximumLength(200).WithMessage(ValidationMessages.MaxLengthIsExceeded);

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage(ValidationMessages.EmptyRequiredField)
            .MaximumLength(1000).WithMessage(ValidationMessages.MaxLengthIsExceeded);
    }
}
