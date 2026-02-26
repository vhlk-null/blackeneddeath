using Library.API.Resources.ResourceManagement;
using Library.Domain.Enums;
using Library.Domain.Models;
using Library.Infrastructure.Data;

namespace Library.API.Albums.CreateAlbum;

public record CreateAlbumCommand(
    string Title,
    int ReleaseDate,
    AlbumType Type,
    AlbumFormat Format,
    string? Label,
    Guid? CountryId,
    List<Guid> BandIds,
    List<Guid> GenreIds,
    List<Guid> TagIds,
    IFormFile? Cover
) : ICommand<CreateAlbumResult>;    

public record CreateAlbumResult(Guid Id);

public class CreateAlbumCommandValidator : AbstractValidator<CreateAlbumCommand>
{
    public CreateAlbumCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage(ValidationMessages.EmptyRequiredField)
            .MaximumLength(200).WithMessage(ValidationMessages.MaxLengthIsExceeded);

        RuleFor(x => x.Label)
            .NotEmpty().WithMessage(ValidationMessages.EmptyRequiredField)
            .MaximumLength(200).WithMessage(ValidationMessages.MaxLengthIsExceeded);

        RuleFor(x => x.CountryId)
            .NotEmpty().WithMessage(ValidationMessages.EmptyRequiredField)
            .WithMessage(ValidationMessages.MaxLengthIsExceeded);

        RuleFor(x => x.ReleaseDate)
            .GreaterThan(0).WithMessage(ValidationMessages.ReleaseYearRequired)
            .GreaterThan(1900).WithMessage(ValidationMessages.ReleaseYearTooOld)
            .LessThanOrEqualTo(DateTime.UtcNow.Year).WithMessage(ValidationMessages.ReleaseYearInFuture);
    }
}

internal class CreateAlbumCommandHandler(IRepository<LibraryContext> repo) 
    : ICommandHandler<CreateAlbumCommand, CreateAlbumResult>
{
    public async ValueTask<CreateAlbumResult> Handle(CreateAlbumCommand command, CancellationToken cancellationToken)
    {
        var album = Album.Create("", AlbumType.Compilation, null, "", null);

        await repo.AddAsync(album, cancellationToken);

        return new CreateAlbumResult(new Guid());
    }
}