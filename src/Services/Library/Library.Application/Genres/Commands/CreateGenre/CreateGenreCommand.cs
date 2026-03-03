namespace Library.Application.Genres.Commands.CreateGenre;

public record CreateGenreCommand(string Name, Guid? ParentGenreId) : BuildingBlocks.CQRS.ICommand<CreateGenreResult>;

public record CreateGenreResult(Guid Id);

public class CreateGenreCommandValidator : AbstractValidator<CreateGenreCommand>
{
    public CreateGenreCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(ValidationMessages.EmptyRequiredField)
            .MaximumLength(200).WithMessage(ValidationMessages.MaxLengthIsExceeded);
    }
}
