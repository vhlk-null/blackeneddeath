namespace Library.Application.Services.Albums.Commands.CreateAlbum;

public record CreateAlbumCommand(AlbumDto Album, Stream? CoverImage = null, string? CoverImageContentType = null, string? CoverImageFileName = null) : BuildingBlocks.CQRS.ICommand<CreateAlbumResult>;

public record CreateAlbumResult(Guid Id);

public class CreateAlbumCommandValidator : AbstractValidator<CreateAlbumCommand>
{
    public CreateAlbumCommandValidator()
    {
        RuleFor(x => x.Album.Title)
            .NotEmpty().WithMessage(ValidationMessages.EmptyRequiredField)
            .MaximumLength(200).WithMessage(ValidationMessages.MaxLengthIsExceeded);

        RuleFor(x => x.Album.Label)
            .NotEmpty().WithMessage(ValidationMessages.EmptyRequiredField)
            .MaximumLength(200).WithMessage(ValidationMessages.MaxLengthIsExceeded);

        RuleFor(x => x.Album.Countries)
            .NotEmpty().WithMessage(ValidationMessages.EmptyRequiredField)
            .WithMessage(ValidationMessages.MaxLengthIsExceeded);

        RuleFor(x => x.Album.ReleaseDate)
            .GreaterThan(0).WithMessage(ValidationMessages.ReleaseYearRequired)
            .GreaterThan(1900).WithMessage(ValidationMessages.ReleaseYearTooOld)
            .LessThanOrEqualTo(DateTime.UtcNow.Year).WithMessage(ValidationMessages.ReleaseYearInFuture);
    }
}