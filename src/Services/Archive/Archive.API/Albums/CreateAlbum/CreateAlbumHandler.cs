namespace Archive.API.Albums.CreateAlbum
{
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

    internal class CreateAlbumCommandHandler(IRepository<ArchiveContext> repo) 
        : ICommandHandler<CreateAlbumCommand, CreateAlbumResult>
    {
        public async Task<CreateAlbumResult> Handle(CreateAlbumCommand command, CancellationToken cancellationToken)
        {
            var album = new Album
            {
                Id = Guid.NewGuid(),
                Title = command.Title,
                ReleaseDate = command.ReleaseDate,
                Type = command.Type,
                Format = command.Format,
                Label = command.Label,
                //CountryId = command.CountryId,
                //CoverUrl = coverUrl,
                //Genres = command.GenreIds.Select(genreId => new AlbumGenre
                //{
                //    GenreId = genreId
                //}).ToList()
            };

            await repo.AddAsync(album, cancellationToken);

            return new CreateAlbumResult(album.Id);
        }
    }
}
