namespace Library.Application.Genres.Commands.DeleteGenre;

public record DeleteGenreCommand(Guid Id) : BuildingBlocks.CQRS.ICommand<DeleteGenreResult>;

public record DeleteGenreResult(bool IsSuccess);

public class DeleteGenreCommandValidator : AbstractValidator<DeleteGenreCommand>
{
    public DeleteGenreCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage(ValidationMessages.EmptyRequiredField);
    }
}
