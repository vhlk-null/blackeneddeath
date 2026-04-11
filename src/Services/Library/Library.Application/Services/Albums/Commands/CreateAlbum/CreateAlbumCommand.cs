namespace Library.Application.Services.Albums.Commands.CreateAlbum;

public record CreateAlbumCommand(CreateAlbumDto Album, Stream? CoverImage = null, string? CoverImageContentType = null, string? CoverImageFileName = null) : BuildingBlocks.CQRS.ICommand<CreateAlbumResult>;

public record CreateAlbumResult(Guid Id);

public class CreateAlbumCommandValidator : AbstractValidator<CreateAlbumCommand>
{
    public CreateAlbumCommandValidator()
    {
        RuleFor(x => x.Album).NotNull();

        When(x => x.Album is not null, () =>
        {
            RuleFor(x => x.Album.Title)
                .NotEmpty().WithMessage(ValidationMessages.EmptyRequiredField)
                .MaximumLength(200).WithMessage(ValidationMessages.MaxLengthIsExceeded);

            RuleFor(x => x.Album.CountryIds)
                .NotEmpty().WithMessage(ValidationMessages.EmptyRequiredField);

            RuleFor(x => x.Album.ReleaseDate)
                .GreaterThanOrEqualTo(1960).WithMessage(ValidationMessages.ReleaseYearTooOld)
                .LessThanOrEqualTo(DateTime.UtcNow.Year + 1).WithMessage(ValidationMessages.ReleaseYearInFuture);
        });
    }
}