namespace Library.Application.Genres.Commands.UpdateGenre;

public record UpdateGenreCommand(Guid Id, string Name, Guid? ParentGenreId) : BuildingBlocks.CQRS.ICommand<UpdateGenreResult>;

public record UpdateGenreResult(bool IsSuccess);

public class UpdateGenreCommandValidator : AbstractValidator<UpdateGenreCommand>
{
    public UpdateGenreCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage(ValidationMessages.EmptyRequiredField);

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(ValidationMessages.EmptyRequiredField)
            .MaximumLength(200).WithMessage(ValidationMessages.MaxLengthIsExceeded);
    }
}
